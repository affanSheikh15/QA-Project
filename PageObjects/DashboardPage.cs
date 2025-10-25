using OpenQA.Selenium;
using QA_Project.Utilities;

namespace QA_Project.PageObjects;

/// <summary>
/// Page Object for Notion Dashboard/Home Page
/// </summary>
public class DashboardPage : BasePage
{
    // Locators
    private readonly By _userAvatar = By.CssSelector("div[class*='notion-topbar'] img[class*='user'], [data-testid='user-avatar']");
    private readonly By _sidebarMenu = By.CssSelector("div[class*='notion-sidebar'], aside[class*='sidebar']");
    private readonly By _newPageButton = By.XPath("//button[contains(., 'New page') or contains(@aria-label, 'New page')]");
    private readonly By _searchButton = By.CssSelector("div[class*='notion-topbar'] div[class*='search'], button[class*='search']");
    private readonly By _workspaceSelector = By.CssSelector("div[class*='notion-topbar'] div[class*='workspace']");
    private readonly By _settingsButton = By.XPath("//button[contains(., 'Settings') or @aria-label='Settings']");
    private readonly By _recentPages = By.CssSelector("div[class*='notion-sidebar'] div[class*='recent']");
    private readonly By _pagesList = By.CssSelector("div[class*='notion-sidebar'] div[class*='page']");
    private readonly By _loadingSpinner = By.CssSelector("div[class*='loading'], div[class*='spinner']");

    public DashboardPage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Wait for dashboard to load completely
    /// </summary>
    public DashboardPage WaitForDashboardLoad()
    {
        WaitForPageLoad();

        // Wait for loading spinner to disappear if present
        try
        {
            WaitHelper.WaitForElementInvisible(Driver, _loadingSpinner, 10);
        }
        catch
        {
            // Spinner might not appear, continue
        }

        return this;
    }

    /// <summary>
    /// Check if user is logged in by verifying dashboard elements
    /// </summary>
    public bool IsUserLoggedIn()
    {
        return IsElementDisplayed(_sidebarMenu, 10) &&
               GetCurrentUrl().Contains("notion.com");
    }

    /// <summary>
    /// Check if user avatar is displayed
    /// </summary>
    public bool IsUserAvatarDisplayed()
    {
        return IsElementDisplayed(_userAvatar, 10);
    }

    /// <summary>
    /// Check if sidebar is displayed
    /// </summary>
    public bool IsSidebarDisplayed()
    {
        return IsElementDisplayed(_sidebarMenu, 5);
    }

    /// <summary>
    /// Check if New Page button is displayed
    /// </summary>
    public bool IsNewPageButtonDisplayed()
    {
        return IsElementDisplayed(_newPageButton, 5);
    }

    /// <summary>
    /// Click New Page button
    /// </summary>
    public DashboardPage ClickNewPage()
    {
        Click(_newPageButton);
        return this;
    }

    /// <summary>
    /// Open search
    /// </summary>
    public DashboardPage OpenSearch()
    {
        Click(_searchButton);
        return this;
    }

    /// <summary>
    /// Click workspace selector
    /// </summary>
    public DashboardPage ClickWorkspaceSelector()
    {
        Click(_workspaceSelector);
        return this;
    }

    /// <summary>
    /// Open settings
    /// </summary>
    public DashboardPage OpenSettings()
    {
        Click(_settingsButton);
        return this;
    }

    /// <summary>
    /// Check if recent pages section is visible
    /// </summary>
    public bool AreRecentPagesVisible()
    {
        return IsElementDisplayed(_recentPages, 5);
    }

    /// <summary>
    /// Get count of pages in sidebar
    /// </summary>
    public int GetPagesCount()
    {
        try
        {
            var elements = Driver.FindElements(_pagesList);
            return elements.Count;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Verify dashboard URL
    /// </summary>
    public bool IsDashboardUrl()
    {
        string url = GetCurrentUrl();
        return url.Contains("notion.com") && !url.Contains("/login");
    }

    /// <summary>
    /// Get dashboard page title
    /// </summary>
    public string GetDashboardTitle()
    {
        return GetPageTitle();
    }
}
