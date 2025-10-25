using OpenQA.Selenium;
using QA_Project.Config;

namespace QA_Project.Utilities;

/// <summary>
/// Helper class for managing explicit waits in Selenium
/// </summary>
public static class WaitHelper
{
    /// <summary>
    /// Waits for an element to be visible on the page
    /// </summary>
    public static IWebElement WaitForElementVisible(IWebDriver driver, By locator, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                var element = driver.FindElement(locator);
                if (element.Displayed)
                {
                    return element;
                }
            }
            catch (NoSuchElementException)
            {
                // Element not found yet, continue waiting
            }
            catch (StaleElementReferenceException)
            {
                // Element is stale, continue waiting
            }

            Thread.Sleep(500); // Wait 500ms before retry
        }

        throw new WebDriverTimeoutException($"Element not visible after {timeout} seconds: {locator}");
    }

    /// <summary>
    /// Waits for an element to be clickable
    /// </summary>
    public static IWebElement WaitForElementClickable(IWebDriver driver, By locator, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                var element = driver.FindElement(locator);
                if (element.Displayed && element.Enabled)
                {
                    return element;
                }
            }
            catch (NoSuchElementException)
            {
                // Element not found yet, continue waiting
            }
            catch (StaleElementReferenceException)
            {
                // Element is stale, continue waiting
            }

            Thread.Sleep(500); // Wait 500ms before retry
        }

        throw new WebDriverTimeoutException($"Element not clickable after {timeout} seconds: {locator}");
    }

    /// <summary>
    /// Waits for an element to exist in the DOM
    /// </summary>
    public static IWebElement WaitForElementExists(IWebDriver driver, By locator, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                return driver.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                // Element not found yet, continue waiting
            }

            Thread.Sleep(500); // Wait 500ms before retry
        }

        throw new WebDriverTimeoutException($"Element not found after {timeout} seconds: {locator}");
    }

    /// <summary>
    /// Waits for an element to be invisible or not present
    /// </summary>
    public static bool WaitForElementInvisible(IWebDriver driver, By locator, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                var element = driver.FindElement(locator);
                if (!element.Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return true; // Element not in DOM = invisible
            }
            catch (StaleElementReferenceException)
            {
                return true; // Element is stale = invisible
            }

            Thread.Sleep(500); // Wait 500ms before retry
        }

        return false; // Timeout - element still visible
    }

    /// <summary>
    /// Waits for page title to contain specific text
    /// </summary>
    public static bool WaitForTitleContains(IWebDriver driver, string title, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            if (driver.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            Thread.Sleep(500);
        }

        return false;
    }

    /// <summary>
    /// Waits for URL to contain specific text
    /// </summary>
    public static bool WaitForUrlContains(IWebDriver driver, string urlPart, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            if (driver.Url.Contains(urlPart, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            Thread.Sleep(500);
        }

        return false;
    }

    /// <summary>
    /// Waits for custom condition with retry logic
    /// </summary>
    public static bool WaitForCondition(IWebDriver driver, Func<IWebDriver, bool> condition, int timeoutSeconds = 0)
    {
        int timeout = timeoutSeconds > 0 ? timeoutSeconds : TestSettings.ExplicitWaitSeconds;
        var endTime = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                if (condition(driver))
                {
                    return true;
                }
            }
            catch
            {
                // Ignore exceptions during condition check
            }

            Thread.Sleep(500);
        }

        return false;
    }

    /// <summary>
    /// Simple thread sleep (use sparingly)
    /// </summary>
    public static void Sleep(int seconds)
    {
        Thread.Sleep(TimeSpan.FromSeconds(seconds));
    }
}