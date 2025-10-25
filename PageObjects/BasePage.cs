using OpenQA.Selenium;
using QA_Project.Utilities;

namespace QA_Project.PageObjects;

/// <summary>
/// Base Page Object containing common methods for all pages
/// </summary>
public abstract class BasePage
{
    protected IWebDriver Driver { get; }

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
    }

    /// <summary>
    /// Navigate to a specific URL and wait for the page to load (with retries)
    /// </summary>
    protected void NavigateTo(string url)
    {
        const int maxAttempts = 3;
        int attempt = 0;
        Exception? lastEx = null;

        while (attempt++ < maxAttempts)
        {
            try
            {
                Console.WriteLine($"🌐 Navigating to: {url} (attempt {attempt}/{maxAttempts})");
                Driver.Navigate().GoToUrl(url);
                WaitForPageLoad(40); // will throw WebDriverTimeoutException if not loaded
                Console.WriteLine("✅ Page loaded successfully.");
                return;
            }
            catch (WebDriverTimeoutException wex)
            {
                lastEx = wex;
                Console.WriteLine($"⚠️ Page load timeout on attempt {attempt}: {wex.Message}");
                // small wait and retry
                Thread.Sleep(1500);
                continue;
            }
            catch (WebDriverException wex2)
            {
                lastEx = wex2;
                Console.WriteLine($"⚠️ WebDriver exception during navigation (attempt {attempt}): {wex2.Message}");
                Thread.Sleep(1500);
                continue;
            }
        }

        // If we get here, all retries failed
        Console.WriteLine($"❌ Failed to navigate to {url} after {maxAttempts} attempts.");
        if (lastEx != null) throw lastEx;
        throw new WebDriverTimeoutException($"Failed to navigate to {url}");
    }

    /// <summary>
    /// Wait for page to load completely
    /// </summary>
    protected void WaitForPageLoad(int timeoutSeconds = 40)
    {
        var endTime = DateTime.Now.AddSeconds(timeoutSeconds);

        while (DateTime.Now < endTime)
        {
            try
            {
                var readyState = ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState");
                if (readyState?.ToString() == "complete")
                {
                    return;
                }
            }
            catch (WebDriverException)
            {
                // driver may be busy — loop and retry until timeout
            }

            Thread.Sleep(500);
        }

        throw new WebDriverTimeoutException("❌ Page did not load completely within the timeout period.");
    }

    /// <summary>
    /// Get current page URL
    /// </summary>
    protected string GetCurrentUrl() => Driver.Url;

    /// <summary>
    /// Get current page title
    /// </summary>
    protected string GetPageTitle() => Driver.Title;

    /// <summary>
    /// Click an element with wait
    /// </summary>
    protected void Click(By locator)
    {
        var element = WaitHelper.WaitForElementClickable(Driver, locator);
        element.Click();
    }

    /// <summary>
    /// Enter text into an input field
    /// </summary>
    protected void EnterText(By locator, string text)
    {
        var element = WaitHelper.WaitForElementVisible(Driver, locator);
        element.Clear();
        element.SendKeys(text);
    }

    /// <summary>
    /// Get text from an element
    /// </summary>
    protected string GetText(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(Driver, locator);
        return element.Text;
    }

    /// <summary>
    /// Check if element is displayed
    /// </summary>
    protected bool IsElementDisplayed(By locator, int timeoutSeconds = 5)
    {
        try
        {
            var element = WaitHelper.WaitForElementVisible(Driver, locator, timeoutSeconds);
            return element.Displayed;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Check if element exists in DOM
    /// </summary>
    protected bool IsElementPresent(By locator)
    {
        try
        {
            Driver.FindElement(locator);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    /// <summary>
    /// Scroll to element
    /// </summary>
    protected void ScrollToElement(By locator)
    {
        var element = WaitHelper.WaitForElementExists(Driver, locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }

    /// <summary>
    /// Execute JavaScript
    /// </summary>
    protected object? ExecuteScript(string script, params object[] args)
    {
        return ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
    }
}
