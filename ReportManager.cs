using System;
using System.IO;

namespace QA_Project
{
    public static class ReportManager
    {
        private static readonly string reportPath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.FullName,
            "Reports",
            "TestReport.txt"
        );

        private static readonly object lockObj = new();

        static ReportManager()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);

            // 🧠 Only create new file if it doesn't exist
            if (!File.Exists(reportPath))
            {
                File.WriteAllText(reportPath,
                    $"Test Report Log\n---------------------------------\n");
            }

            // 🕒 Add header for each new test run
            File.AppendAllText(reportPath,
                $"\n--- New Test Run: {DateTime.Now} ---\n");
        }

        public static void LogResult(string testName, bool passed, string? errorMessage = "")
        {
            lock (lockObj)
            {
                using var writer = File.AppendText(reportPath);
                writer.WriteLine($"{DateTime.Now:HH:mm:ss} | {(passed ? "[PASS]" : "[FAIL]")} | {testName}");

                if (!passed && !string.IsNullOrEmpty(errorMessage))
                {
                    writer.WriteLine($"   ↳ Error: {errorMessage}");
                }
            }
        }
    }
}
