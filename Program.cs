using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ArmelloLogTools.Armello;
using MoreLinq.Extensions;
using Timer = System.Timers.Timer;

namespace ArmelloLogTools
{
    internal static class Program
    {
        private static LogReader _reader;
        private static Ui _ui;
        private static Interpreter _interpreter;


        private static IEnumerable<Event> FindLastGame(IList<Event> events)
        {
            for (var i = events.Count - 1; i >= 0; i--)
            {
                if (events[i].Type == EventType.LoadGame)
                {
                    return events.Skip(i);
                }
            }

            throw new Exception("Cannot find a game");
        }

        private static void Main(string[] args)
        {
            var logFile = args.Length > 0 ? args[0] : LogFile.LatestLogFile();

            _reader = new LogReader(logFile);
            _interpreter = new Interpreter();
            _ui = new Ui {LogFilename = logFile};
            
            UpdateEventsAndUi(true);

            using var timer = new Timer(5000) {AutoReset = true};

            timer.Elapsed += (sender, eventArgs) => { UpdateEventsAndUi(); };

            timer.Start();

            ExitLoop();

            _reader.Dispose();
        }

        private static void UpdateEventsAndUi(bool lastGame = false)
        {
            var events = Parser.ParseLines(_reader.ReadLines(), ParserTest.QuestCounterTests);

            if (lastGame)
            {
                events = FindLastGame(events).ToList();
            }

            //No events, no need to update the UI
            if (events.Count == 0) return;

            _interpreter.ProcessEvents(events);
            _ui.Players = _interpreter.Players.Select(pair => pair.Value);
            _ui.Draw();
        }

        private static void ExitLoop()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (!Console.KeyAvailable) continue;

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) break;
            }
        }
    }
}