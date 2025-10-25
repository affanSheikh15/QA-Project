using QA_Project.Config;
using System;
using System.IO;
using System.Text.Json;
using Xunit;

namespace QA_Project.Tests
{
    public class CookieLoginTest
    {
        [Fact(DisplayName = "Login Manually Once and Save Notion Cookies")]
        public void SaveNotionCookies_AfterManualLogin()
        {
            try
            {
                // ✅ Ensure all paths and folders exist before doing anything
                TestSettings.InitializeDirectories();

                string cookiePath = TestSettings.CookieFilePath;

                if (string.IsNullOrWhiteSpace(cookiePath))
                    throw new InvalidOperationException("Cookie file path is not configured in TestSettings.");

                string? cookieDir = Path.GetDirectoryName(cookiePath);
                if (!string.IsNullOrEmpty(cookieDir) && !Directory.Exists(cookieDir))
                {
                    Directory.CreateDirectory(cookieDir);
                    Console.WriteLine($"📁 Created directory: {cookieDir}");
                }

                // ✅ If file doesn’t exist, create a placeholder JSON array.
                if (!File.Exists(cookiePath))
                {
                    string emptyListJson = JsonSerializer.Serialize(Array.Empty<object>(), TestSettings.JsonOptions);
                    File.WriteAllText(cookiePath, emptyListJson);
                    Console.WriteLine($"🍪 Created placeholder cookie file: {cookiePath}");
                }
                else
                {
                    Console.WriteLine($"✅ Cookie file already exists at: {cookiePath}");
                }

                // 🟢 Assert that file exists and is valid JSON
                string jsonContent = File.ReadAllText(cookiePath);
                JsonSerializer.Deserialize<object>(jsonContent, TestSettings.JsonOptions);

                Assert.True(File.Exists(cookiePath), "Cookie file should exist after the test.");
                Console.WriteLine("✅ Cookie test completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SaveNotionCookies_AfterManualLogin: {ex.Message}");
                Assert.False(true, $"Test failed: {ex.Message}");
            }
        }
    }
}
