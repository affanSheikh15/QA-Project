using QA_Project.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;

namespace QA_Project.Drivers
{
    public static class DriverFactory
    {
        private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(180);

        public static IWebDriver CreateDriver(string browserType = "Chrome", bool headless = true)
        {
            headless = true; // Keep headless by default

            IWebDriver driver = browserType.ToLower() switch
            {
                "chrome" => CreateChromeDriver(headless),
                "edge" => CreateEdgeDriver(headless),
                _ => throw new ArgumentException($"Browser type '{browserType}' is not supported. Use 'Chrome' or 'Edge'.")
            };

            ConfigureDriverTimeouts(driver);

            if (!headless)
            {
                try { driver.Manage().Window.Maximize(); } catch { }
            }

            Console.WriteLine($"🚀 Browser launched in headless mode: {browserType}");
            return driver;
        }

        private static ChromeDriver CreateChromeDriver(bool headless)
        {
            var options = new ChromeOptions();
            options.AddArguments(
                "--no-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu",
                "--disable-extensions",
                "--disable-infobars",
                "--disable-notifications",
                "--disable-popup-blocking",
                "--window-size=1920,1080",
                "--disable-renderer-backgrounding", // ✅ Prevent Chrome renderer from freezing
                "--start-maximized"
            );

            if (headless)
                options.AddArguments("--headless=new");

            options.PageLoadStrategy = PageLoadStrategy.Normal; // ✅ Ensures full load

            // Anti-detection & stability
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                "Chrome/120.0.0.0 Safari/537.36");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            var driver = new ChromeDriver(service, options, CommandTimeout);

            try
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");
            }
            catch { }

            return driver;
        }

        private static EdgeDriver CreateEdgeDriver(bool headless)
        {
            var options = new EdgeOptions();
            options.AddArguments(
                "--no-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu",
                "--disable-extensions",
                "--disable-infobars",
                "--disable-notifications",
                "--disable-popup-blocking",
                "--window-size=1920,1080",
                "--disable-renderer-backgrounding", // ✅ Stability for Edge
                "--start-maximized"
            );

            if (headless)
                options.AddArguments("--headless=new");

            options.PageLoadStrategy = PageLoadStrategy.Normal;

            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);

            var service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new EdgeDriver(service, options, CommandTimeout);
        }

        private static void ConfigureDriverTimeouts(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TestSettings.ImplicitWaitSeconds);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(
                Math.Max(90, TestSettings.PageLoadTimeoutSeconds) // ✅ Extended to 90s minimum
            );
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(40);
        }
    }
}
