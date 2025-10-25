using OpenQA.Selenium;
using QA_Project.Config;
using QA_Project.PageObjects;
using Xunit;

namespace QA_Project.Tests;

/// <summary>
/// Dashboard functionality test cases
/// Tests dashboard elements and navigation
/// </summary>
public class DashboardTests : BaseTest
{
    private LoginPage _loginPage = null!;
    private DashboardPage _dashboardPage = null!;

    private void Setup(string browserType = "Chrome")
    {
        InitializeDriver(browserType);
        Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120); // ✅ Increased timeout
        _loginPage = new LoginPage(Driver);
        _dashboardPage = new DashboardPage(Driver);
    }

    private void Teardown()
    {
        CleanupDriver();
    }

    private void NavigateToDashboard()
    {
        int retries = 3;
        while (retries-- > 0)
        {
            try
            {
                Driver.Navigate().GoToUrl(TestSettings.NotionBaseUrl);
                Thread.Sleep(3000);
                return;
            }
            catch (WebDriverTimeoutException)
            {
                if (retries == 0)
                    throw;
                Console.WriteLine("⚠ Timeout loading dashboard — retrying...");
                Thread.Sleep(5000);
            }
        }
    }

    private static void AssertUrlContainsNotion(string url)
    {
        Assert.True(
            url.Contains("notion.com") || url.Contains("notion.so"),
            $"Expected Notion URL, but got: {url}"
        );
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "Critical")]
    public void Test_01_DashboardLoad_ShouldDisplaySidebar()
    {
        Setup("Chrome");
        TestName = "Dashboard_Sidebar";

        try
        {
            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            string currentUrl = Driver.Url;
            AssertUrlContainsNotion(currentUrl);

            Console.WriteLine($"✅ Successfully navigated to Notion: {currentUrl}");
        }
        catch (Exception)
        {
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "High")]
    public void Test_02_Dashboard_VerifyPageTitle()
    {
        Setup("Chrome");
        TestName = "Dashboard_PageTitle";

        try
        {
            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            string pageTitle = _dashboardPage.GetDashboardTitle();
            Assert.False(string.IsNullOrEmpty(pageTitle), "Page title should not be empty");
            Assert.Contains("Notion", pageTitle);

            Console.WriteLine($"✅ Page title verified: {pageTitle}");
        }
        catch (Exception)
        {
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "High")]
    public void Test_03_Dashboard_VerifyURL()
    {
        Setup("Chrome");
        TestName = "Dashboard_URL";

        try
        {
            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            string currentUrl = Driver.Url;
            AssertUrlContainsNotion(currentUrl);

            Console.WriteLine("✅ Dashboard URL verified");
        }
        catch (Exception)
        {
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "Medium")]
    public void Test_04_Dashboard_CrossBrowser_Chrome()
    {
        Setup("Chrome");
        TestName = "Dashboard_Chrome";

        try
        {
            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            AssertUrlContainsNotion(Driver.Url);

            Console.WriteLine("✅ Dashboard accessible on Chrome");
        }
        catch (Exception)
        {
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "Medium")]
    public void Test_05_Dashboard_CrossBrowser_Edge()
    {
        Setup("Edge");
        TestName = "Dashboard_Edge";

        try
        {
            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            AssertUrlContainsNotion(Driver.Url);

            Console.WriteLine("✅ Dashboard accessible on Edge");
        }
        catch (Exception)
        {
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }

    [Fact]
    [Trait("Category", "Dashboard")]
    [Trait("Priority", "Medium")]
    public void Test_06_Dashboard_PageLoadPerformance()
    {
        Setup("Chrome");
        TestName = "Dashboard_Performance";

        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            NavigateToDashboard();
            _dashboardPage.WaitForDashboardLoad();

            stopwatch.Stop();
            var loadTime = stopwatch.Elapsed.TotalSeconds;

            Assert.True(loadTime < 60, $"Page should load within 60 seconds (loaded in {loadTime:F2}s)");

            Console.WriteLine($"✅ Page load time: {loadTime:F2} seconds");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error during dashboard load: {ex.Message}");
            CaptureFailureScreenshot(TestName);
            throw;
        }
        finally
        {
            Teardown();
        }
    }
}
