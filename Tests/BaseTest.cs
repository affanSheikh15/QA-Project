using OpenQA.Selenium;
using QA_Project.Config;
using QA_Project.Drivers;
using QA_Project.Utilities;
using System.Text.Json;

namespace QA_Project.Tests
{
    /// <summary>
    /// Base test class with setup and teardown methods
    /// </summary>
    public abstract class BaseTest : IDisposable
    {
        protected IWebDriver Driver { get; private set; } = null!;
        protected string TestName { get; set; } = string.Empty;

        /// <summary>
        /// Initialize WebDriver before each test
        /// </summary>
        protected void InitializeDriver(string browserType = "Chrome")
        {
            try
            {
                string browser = TestSettings.DefaultBrowser;
                if (!string.IsNullOrEmpty(browserType))
                    browser = browserType;

                Driver = DriverFactory.CreateDriver(browser, TestSettings.HeadlessMode);
                Console.WriteLine($"✅ Driver initialized successfully: {browser}");

                // 🟢 Extend page load timeout to 120 seconds (fixes renderer timeout)
                Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);

                // 🟡 Load cookies if available
                if (TestSettings.UseSavedSession && File.Exists(TestSettings.CookieFilePath))
                {
                    try
                    {
                        Driver.Navigate().GoToUrl(TestSettings.NotionBaseUrl);
                        var cookieJson = File.ReadAllText(TestSettings.CookieFilePath);
                        var cookies = JsonSerializer.Deserialize<List<CookieData>>(cookieJson, TestSettings.JsonOptions);

                        if (cookies != null)
                        {
                            foreach (var c in cookies)
                            {
                                var cookie = new Cookie(c.Name, c.Value, c.Domain, c.Path, c.Expiry);
                                Driver.Manage().Cookies.AddCookie(cookie);
                            }

                            Driver.Navigate().Refresh();
                            Console.WriteLine("🍪 Session cookies loaded successfully — skipping login.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Failed to load cookies: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to initialize driver: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Cleanup after each test
        /// </summary>
        protected void CleanupDriver()
        {
            try
            {
                Driver?.Quit();
                Driver?.Dispose();
                Console.WriteLine("✅ Driver cleaned up successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error during driver cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// Capture screenshot on test failure
        /// </summary>
        protected void CaptureFailureScreenshot(string testMethodName)
        {
            if (Driver != null && TestSettings.TakeScreenshotOnFailure)
            {
                try
                {
                    ScreenshotHelper.CaptureOnFailure(Driver, testMethodName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to capture screenshot: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            CleanupDriver();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Helper class for cookie JSON structure
    /// </summary>
    public record CookieData(string Name, string Value, string Domain, string Path = "/", DateTime? Expiry = null);
}
