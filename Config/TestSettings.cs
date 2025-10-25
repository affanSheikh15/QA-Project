using System;
using System.IO;
using System.Text.Json;

namespace QA_Project.Config
{
    /// <summary>
    /// Central test configuration and reusable options
    /// </summary>
    public static class TestSettings
    {
        // 🔗 Notion URLs
        public static string NotionBaseUrl { get; set; } = "https://www.notion.so/";
        public static string NotionLoginUrl { get; set; } = "https://www.notion.so/login";

        // 🌐 Browser Settings
        public static string DefaultBrowser { get; set; } = "Chrome"; // Chrome, Edge, Firefox
        public static bool HeadlessMode { get; set; } = true;
        public static bool UseSavedSession { get; set; } = true;

        // Timeouts
        public static int ImplicitWaitSeconds { get; set; } = 10;
        public static int ExplicitWaitSeconds { get; set; } = 15;
        public static int PageLoadTimeoutSeconds { get; set; } = 60;

        // 🍪 Cookies & Session Management
        public static string CookieFilePath { get; set; } =
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Desktop", "QA-Project", "Data", "cookies.json");
        public static bool SaveCookiesAfterLogin { get; set; } = true;

        // Json options (cached, reused)
        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        // 📸 Screenshot Settings
        public static bool TakeScreenshotOnFailure { get; set; } = true;
        public static string ScreenshotPath { get; set; } =
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Desktop", "QA-Project", "Screenshots");

        // 🧪 Test Metadata — renamed to avoid collision with System.Environment
        public static string TestEnvironment { get; set; } = "QA";
        public static string TesterName { get; set; } = "Affan Sheikh";

        // Example test credentials and invalid inputs referenced by tests
        // (Replace with secure secrets or move to config file as needed)
        public static string ValidEmail { get; set; } = "valid@example.com";
        public static string ValidPassword { get; set; } = "ValidPassword123!";
        public static string InvalidEmail { get; set; } = "invalid@example";
        public static string InvalidPassword { get; set; } = "wrongpass";

        /// <summary>
        /// Ensure required directories exist (cookies + screenshots)
        /// </summary>
        public static void InitializeDirectories()
        {
            try
            {
                var cookieDir = Path.GetDirectoryName(CookieFilePath);
                if (!string.IsNullOrEmpty(cookieDir) && !Directory.Exists(cookieDir))
                    Directory.CreateDirectory(cookieDir);

                if (!Directory.Exists(ScreenshotPath))
                    Directory.CreateDirectory(ScreenshotPath);
            }
            catch
            {
                // swallow: best-effort creation
            }
        }
    }
}
