using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArmelloLogTools.Armello;
using ConsoleTables;

namespace ArmelloLogTools
{
    public class Ui
    {
        public string LogFilename;
        public IEnumerable<Player> Players;

        public void Draw()
        {
            Console.Clear();
            DrawLogFilename();
            DrawQuestTable();
            DrawInstructions();
        }

        private void DrawLogFilename()
        {
            Console.WriteLine($"Using log file : {LogFilename}");
        }

        private void DrawQuestTable()
        {
            //Do nothing if there is no players
            if (Players == null) return;

            var heroesName = new List<string>();
            var quests = new List<string>();

            foreach (var player in Players)
            {
                heroesName.Add(player.Hero.ToString());

                //Quest
                switch (player.QuestCount)
                {
                    case 5:
                        quests.Add("Palace");
                        break;
                    case 6:
                        quests.Add("Done");
                        break;
                    default:
                        quests.Add(player.QuestCount.ToString());
                        break;
                }
            }

            var table = new ConsoleTable(heroesName.ToArray());
            table.AddRow(quests.ToArray());
            var output = table.ToStringAlternative();

            Console.Write(output);
        }

        private static void DrawInstructions()
        {
            Console.WriteLine("Press escape to quit");
        }
    }
}