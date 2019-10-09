using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace ArmelloLogTools
{
    public class LogReader
    {
        private readonly StreamReader _reader;

        public LogReader(string filename)
        {
            _reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public IEnumerable<string> ReadLines()
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }

    public static class LogFile
    {
        private static Regex _armelloLogFilenameDatetimeRegex =
            new Regex(@"^.+_log_(.+)\.txt$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string LatestLogFile(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var files = dir.GetFiles("*.txt");

            return files.Select(info => (info: info, match: _armelloLogFilenameDatetimeRegex.Match(info.Name)))
                .Where(t => t.match.Success)
                .Select(t =>
                    (info: t.info,
                        datetime: DateTime.ParseExact(t.match.Groups[1].Value, "yyyy-MM-dd_hh-mm-ss-tt",
                            CultureInfo.InvariantCulture)))
                .MaxBy(t => t.datetime).First().info.FullName;
        }

        public static string LatestLogFile()
        {
            var armelloLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "../LocalLow/League of Geeks/Armello/logs");
            return LatestLogFile(armelloLogPath);
        }
    }
}