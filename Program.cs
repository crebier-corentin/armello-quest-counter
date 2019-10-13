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

        private static void Main(string[] args)
        {
            var manualLogFile = args.Length > 0;
            var logFile = manualLogFile ? args[0] : LogFile.LatestLogFile();

            using (_reader = new LogReader(logFile))
            {
                _interpreter = new Interpreter();
                _ui = new Ui {LogFilename = logFile};

                UpdateEventsAndUi();

                //Timer
                using var timer = new Timer(5000) {AutoReset = true};
                timer.Elapsed += (sender, eventArgs) => { UpdateEventsAndUi(); };
                timer.Start();

                //Watch for new log files
                using var watcher = new FileSystemWatcher
                {
                    Path = LogFile.LogDirectoryPath,
                    Filter = "*.txt",
                    NotifyFilter = NotifyFilters.LastAccess |
                                   NotifyFilters.LastWrite |
                                   NotifyFilters.FileName |
                                   NotifyFilters.DirectoryName
                };
                if (!manualLogFile)
                {
                    watcher.Created += (sender, eventArgs) =>
                    {
                        //Wait for file to be named
                        Thread.Sleep(1000);

                        var newFilename = LogFile.LatestLogFile();
                        //Ignore if LatestLogFile has not changed
                        if (newFilename == _reader.Filename) return;

                        //Change log file
                        _ui.LogFilename = newFilename;
                        _reader.ChangeFile(newFilename);
                        UpdateEventsAndUi();
                    };

                    watcher.EnableRaisingEvents = true;
                }

                ExitLoop();
            }
        }

        private static void UpdateEventsAndUi()
        {
            var events = Parser.ParseLines(_reader.ReadLines(), ParserTest.QuestCounterTests);
            
            if (events.Count > 0)
            {
                _interpreter.ProcessEvents(events);
                _ui.Players = _interpreter.Players.Select(pair => pair.Value);
            }

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