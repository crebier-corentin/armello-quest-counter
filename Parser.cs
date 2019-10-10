using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ArmelloLogTools.Armello;

namespace ArmelloLogTools
{
    public struct ParserTest
    {
        public readonly Regex Regex;
        public readonly Func<GroupCollection, Event> Evaluator;

        public ParserTest(Regex regex, Func<GroupCollection, Event> evaluator)
        {
            Regex = regex;
            Evaluator = evaluator;
        }

        public static readonly ParserTest[] DefaultTests = new ParserTest[]
        {
            //Load Game
            new ParserTest(
                new Regex(@"---\s+Load\s+Game", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                collection => new LoadGameEvent()),

            //Load Player
            new ParserTest(
                new Regex(
                    @"Gameplay:\s+\[\s+\D+\]\s+Id:\s+Player(\d),\s+Name:\s+(.+),\s+Network\s+Id:\s+.+,\s+Hero:\s+(0x\w+)",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase),
                groups => new LoadPlayerEvent(int.Parse(groups[1].Value), groups[2].Value, groups[3].Value)),

            //Start Turn
            new ParserTest(
                new Regex(
                    @"Player: Player\+Message\+StartTurn: Dispatch\(\[Player .+ \(Player(\d)\)",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase),
                groups => new StartTurnEvent(int.Parse(groups[1].Value))),

            //On Spawn Quest Complete
            new ParserTest(
                new Regex(
                    @"Quest: OnSpawnQuestComplete - player: Player(\d), quest: \w+, questTilePos: \(-?\d+,-?\d+\), success: True",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase),
                groups => new OnQuestSpawnComplete(int.Parse(groups[1].Value))),
            
            //Complete Quest
            new ParserTest(
                new Regex(
                    @"Gameplay: QuestManager\+Message\+CompleteQuest",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase),
                groups => new CompleteQuestEvent()),
        };
    }

    public static class Parser
    {
        public static List<Event> ParseLines(IEnumerable<string> lines)
        {
            var events = new List<Event>();

            foreach (var line in lines)
            {
                TestLine(events, line);
            }

            return events;
        }

        private static void TestLine(ICollection<Event> events, string line)
        {
            foreach (var test in ParserTest.DefaultTests)
            {
                var match = test.Regex.Match(line);

                if (!match.Success) continue;

                events.Add(test.Evaluator(match.Groups));
                break;
            }
        }
    }
}