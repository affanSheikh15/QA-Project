using OpenQA.Selenium;
using QA_Project.Config;

namespace QA_Project.Utilities;

public static class ScreenshotHelper
{
    /// <summary>
    /// Captures and saves a screenshot with timestamp
    /// </summary>
    /// <param name="driver">WebDriver instance</param>
    /// <param name="testName">Name of the test for file naming</param>
    /// <returns>Path to saved screenshot</returns>
    public static string CaptureScreenshot(IWebDriver driver, string testName)
    {
        try
        {
            // Ensure Screenshots directory exists
            TestSettings.InitializeDirectories();

            // Take screenshot
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            // Generate unique filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{testName}_{timestamp}.png";
            var filePath = Path.Combine(TestSettings.ScreenshotPath, fileName);

            // Save screenshot to file
            screenshot.SaveAsFile(filePath);

            Console.WriteLine($"📸 Screenshot saved: {filePath}");
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to capture screenshot: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Captures screenshot specifically for test failures
    /// Adds "FAILED_" prefix to filename
    /// </summary>
    /// <param name="driver">WebDriver instance</param>
    /// <param name="testName">Name of the failed test</param>
    /// <returns>Path to saved screenshot</returns>
    public static string CaptureOnFailure(IWebDriver driver, string testName)
    {
        if (TestSettings.TakeScreenshotOnFailure)
        {
            return CaptureScreenshot(driver, $"FAILED_{testName}");
        }
        return string.Empty;
    }

    /// <summary>
    /// Captures multiple screenshots at different points in test
    /// Useful for debugging complex test scenarios
    /// </summary>
    /// <param name="driver">WebDriver instance</param>
    /// <param name="testName">Base name for the test</param>
    /// <param name="stepName">Name of the step/checkpoint</param>
    /// <returns>Path to saved screenshot</returns>
    public static string CaptureCheckpoint(IWebDriver driver, string testName, string stepName)
    {
        return CaptureScreenshot(driver, $"{testName}_{stepName}");
    }

    /// <summary>
    /// Captures screenshot of specific element only
    /// </summary>
    /// <param name="driver">WebDriver instance</param>
    /// <param name="testName">Name for the screenshot file</param>
    /// <returns>Path to saved screenshot</returns>
    public static string CaptureElementScreenshot(IWebDriver driver, string testName)
    {
        try
        {
            TestSettings.InitializeDirectories();

            // Take full page screenshot first
            var fullScreenshot = ((ITakesScreenshot)driver).GetScreenshot();

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{testName}_Element_{timestamp}.png";
            var filePath = Path.Combine(TestSettings.ScreenshotPath, fileName);

            // Save element screenshot
            fullScreenshot.SaveAsFile(filePath);

            Console.WriteLine($"📸 Element screenshot saved: {filePath}");
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to capture element screenshot: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Clears old screenshots from the Screenshots directory
    /// Useful for cleanup before test runs
    /// </summary>
    /// <param name="olderThanDays">Delete screenshots older than specified days</param>
    public static void CleanupOldScreenshots(int olderThanDays = 7)
    {
        try
        {
            if (Directory.Exists(TestSettings.ScreenshotPath))
            {
                var files = Directory.GetFiles(TestSettings.ScreenshotPath, "*.png");
                var cutoffDate = DateTime.Now.AddDays(-olderThanDays);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                        Console.WriteLine($"🗑️ Deleted old screenshot: {fileInfo.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Failed to cleanup old screenshots: {ex.Message}");
        }
    }
}