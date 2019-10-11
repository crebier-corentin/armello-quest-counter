using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
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

        public ParserTest(string regex, Func<GroupCollection, Event> evaluator)
        {
            Regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Evaluator = evaluator;
        }

        public static readonly ParserTest[] QuestCounterTests = new ParserTest[]
        {
            Tests.LoadGame,
            Tests.LoadPlayer,
            Tests.OnSpawnQuestComplete
        };

        public static class Tests
        {
            public static ParserTest LoadGame = new ParserTest(@"---\s+Load\s+Game", groups => new LoadGameEvent());

            public static ParserTest LoadPlayer =
                new ParserTest(
                    @"Gameplay:\s+\[\s+\D+\]\s+Id:\s+Player(\d),\s+Name:\s+(.+),\s+Network\s+Id:\s+.+,\s+Hero:\s+(0x\w+)",
                    groups => new LoadPlayerEvent(int.Parse(groups[1].Value), groups[2].Value, groups[3].Value));

            public static ParserTest StartTurn =
                new ParserTest(
                    @"Player: Player\+Message\+StartTurn: Dispatch\(\[Player .+ \(Player(\d)\)",
                    groups => new StartTurnEvent(int.Parse(groups[1].Value)));

            public static ParserTest OnSpawnQuestComplete =
                new ParserTest(
                    @"Quest: OnSpawnQuestComplete - player: Player(\d), quest: \w+, questTilePos: \(-?\d+,-?\d+\), success: True",
                    groups => new OnQuestSpawnComplete(int.Parse(groups[1].Value)));

            public static ParserTest CompleteQuest =
                new ParserTest(
                    @"Gameplay: QuestManager\+Message\+CompleteQuest",
                    groups => new CompleteQuestEvent());
        }
    }

    public static class Parser
    {
        public static List<Event> ParseLines(IEnumerable<string> lines, IEnumerable<ParserTest> tests)
        {
            var events = new List<Event>();

            foreach (var line in lines)
            {
                TestLine(events, line, tests);
            }

            return events;
        }

        private static void TestLine(ICollection<Event> events, string line, IEnumerable<ParserTest> tests)
        {
            foreach (var test in tests)
            {
                var match = test.Regex.Match(line);

                if (!match.Success) continue;

                events.Add(test.Evaluator(match.Groups));
                break;
            }
        }
    }
}