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
        private int _cursorLeftPosition = -1;
        private int _cursorTopPosition = -1;

        public string LogFilename;
        public IEnumerable<Player> Players;

        public void Draw()
        {
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

            Console.Clear();
            DrawLogFilename();
            DrawTable(heroesName, quests);
            DrawInstructions();
        }

        private void DrawLogFilename()
        {
            Console.WriteLine($"Using log file : {LogFilename}");
        }

        private void DrawTable(IEnumerable<string> columns, IEnumerable<string> line1)
        {
            if (_cursorLeftPosition == -1 || _cursorTopPosition == -1)
            {
                _cursorLeftPosition = Console.CursorLeft;
                _cursorTopPosition = Console.CursorTop;
            }

            var left = Console.CursorLeft;
            var top = Console.CursorTop;

            Console.SetCursorPosition(_cursorLeftPosition, _cursorTopPosition);

            var table = new ConsoleTable(columns.ToArray());
            table.AddRow(line1.ToArray());
            var output = table.ToStringAlternative();

            Console.Write(output);

            if (top > _cursorTopPosition)
            {
                Console.SetCursorPosition(left, top);
            }
        }

        private static void DrawInstructions()
        {
            Console.WriteLine("Press escape to quit");
        }
    }
}