using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace ArmelloLogTools
{
    public class LogReader : IDisposable
    {
        private StreamReader _reader;
        public string Filename { get; private set; }

        public LogReader(string filename)
        {
            Filename = filename;
            _reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public void ChangeFile(string filename)
        {
            Filename = filename;
            //Close previous
            _reader.Dispose();
            //Open new file
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

        public void Dispose()
        {
            _reader.Dispose();
        }
    }

    public static class LogFile
    {
        private static Regex _armelloLogFilenameDatetimeRegex =
            new Regex(@"^.+_log_(\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}-(?:A|P)M)\.txt$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly string LogDirectoryPath = GetLogDirectoryPath();

        public static string LatestLogFile(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var files = dir.GetFiles("*.txt");

            return files.Select(info => (info, match: _armelloLogFilenameDatetimeRegex.Match(info.Name)))
                .Where(t => t.match.Success)
                .Select(t =>
                    (t.info,
                        datetime: DateTime.ParseExact(t.match.Groups[1].Value, "yyyy-MM-dd_hh-mm-ss-tt",
                            CultureInfo.InvariantCulture)))
                .MaxBy(t => t.datetime).First().info.FullName;
        }

        public static string LatestLogFile()
        {
            return LatestLogFile(LogDirectoryPath);
        }

        private static string GetLogDirectoryPath()
        {
            var relativePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "../LocalLow/League of Geeks/Armello/logs");

            var dir = new DirectoryInfo(relativePath);

            return dir.FullName;
        }
    }
}