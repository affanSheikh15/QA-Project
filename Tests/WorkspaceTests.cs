using OpenQA.Selenium;
using QA_Project.Config;
using QA_Project.PageObjects;
using Xunit;

namespace QA_Project.Tests;

/// <summary>
/// Workspace functionality test cases
/// Tests workspace-related features and navigation
/// </summary>
public class WorkspaceTests : BaseTest
{
    private WorkspacePage _workspacePage = null!;

    private void Setup(string browserType = "Chrome")
    {
        InitializeDriver(browserType);
        _workspacePage = new WorkspacePage(Driver);
    }

    private void Teardown()
    {
        CleanupDriver();
    }

    private void NavigateToWorkspace()
    {
        Driver.Navigate().GoToUrl(TestSettings.NotionBaseUrl);
        Thread.Sleep(3000);
    }

    private static void AssertUrlContainsNotion(string url)
    {
        Assert.True(
            url.Contains("notion.com") || url.Contains("notion.so"),
            $"Expected Notion URL, but got: {url}"
        );
    }

    [Fact]
    [Trait("Category", "Workspace")]
    [Trait("Priority", "High")]
    public void Test_01_WorkspacePage_ShouldLoad()
    {
        Setup("Chrome");
        TestName = "Workspace_Load";

        try
        {
            NavigateToWorkspace();

            string currentUrl = Driver.Url;
            AssertUrlContainsNotion(currentUrl);

            Console.WriteLine("✅ Workspace page loaded successfully");
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
    [Trait("Category", "Workspace")]
    [Trait("Priority", "Medium")]
    public void Test_02_Workspace_VerifyURL()
    {
        Setup("Chrome");
        TestName = "Workspace_URL";

        try
        {
            NavigateToWorkspace();

            string currentUrl = Driver.Url;
            AssertUrlContainsNotion(currentUrl);

            Console.WriteLine($"✅ Workspace URL verified: {currentUrl}");
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
    [Trait("Category", "Workspace")]
    [Trait("Priority", "Medium")]
    public void Test_03_Workspace_CrossBrowser_Chrome()
    {
        Setup("Chrome");
        TestName = "Workspace_Chrome";

        try
        {
            NavigateToWorkspace();
            AssertUrlContainsNotion(Driver.Url);

            Console.WriteLine("✅ Workspace accessible on Chrome");
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
    [Trait("Category", "Workspace")]
    [Trait("Priority", "Medium")]
    public void Test_04_Workspace_CrossBrowser_Edge()
    {
        Setup("Edge");
        TestName = "Workspace_Edge";

        try
        {
            NavigateToWorkspace();
            AssertUrlContainsNotion(Driver.Url);

            Console.WriteLine("✅ Workspace accessible on Edge");
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
    [Trait("Category", "Workspace")]
    [Trait("Priority", "Low")]
    public void Test_05_Workspace_PageTitle()
    {
        Setup("Chrome");
        TestName = "Workspace_Title";

        try
        {
            NavigateToWorkspace();

            string pageTitle = Driver.Title;
            Assert.False(string.IsNullOrEmpty(pageTitle), "Page should have a title");

            Console.WriteLine($"✅ Workspace page title: {pageTitle}");
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
}
