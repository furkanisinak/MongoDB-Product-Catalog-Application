using System;
using System.IO;
using System.Collections.Generic;

namespace Program.Utils
{
    public class LogControl
    {
        private readonly string _FolderPath = "log";
        private readonly string _FilePath;

        public LogControl()
        {
            FolderControlOrCreate();
            DateTime date = DateTime.Now;
            
            _FilePath = $"{_FolderPath}/{date.Year}_{date.Month:D2}_{date.Day:D2}.txt"; // 2025_05_26.txt günün tarihini vericek
        }

        private void FolderControlOrCreate()
        {
            if (!Directory.Exists(_FolderPath))
            {
                Directory.CreateDirectory(_FolderPath);
            }
        }


        public void LogError(string methodName, Exception ex)
        {
            // Zaman damgası ve metot adını da içeren log mesajı formatı
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Method: {methodName}, Hata: {ex.Message}";
            string line = Environment.NewLine;
            File.AppendAllText(_FilePath, $"{logMessage}{line}");

        }

        public List<string> GetAllLogs()
        {
            List<string> list = new();
            if (File.Exists(_FilePath))
            {
                using var reader = new StreamReader(_FilePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }

        public List<string> SearchInLogs(string searchText)
        {
            List<string> matchingLogs = new();
            if (File.Exists(_FilePath))
            {
                using var reader = new StreamReader(_FilePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(searchText))
                    {
                        matchingLogs.Add(line);
                    }
                }
            }
            return matchingLogs;
        }
    }
}