using OpenQA.Selenium;
using QA_Project.Utilities;

namespace QA_Project.PageObjects;

/// <summary>
/// Page Object for Notion Workspace Actions
/// </summary>
public class WorkspacePage : BasePage
{
    // Locators
    private readonly By _workspaceName = By.CssSelector("div[class*='notion-topbar'] div[class*='workspace-name']");
    private readonly By _workspaceMenu = By.CssSelector("div[role='menu'], div[class*='workspace-menu']");
    private readonly By _workspaceSettings = By.XPath("//div[contains(text(), 'Settings') or contains(text(), 'Workspace settings')]");
    private readonly By _addMemberButton = By.XPath("//button[contains(., 'Add member') or contains(., 'Invite')]");
    private readonly By _membersList = By.CssSelector("div[class*='members'], div[class*='team-members']");
    private readonly By _workspacePages = By.CssSelector("div[class*='notion-sidebar'] a[class*='page']");
    private readonly By _createNewButton = By.CssSelector("button[class*='create'], button[aria-label*='Create']");
    private readonly By _importButton = By.XPath("//button[contains(., 'Import')]");
    private readonly By _trashButton = By.XPath("//button[contains(., 'Trash') or @aria-label='Trash']");

    public WorkspacePage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Get current workspace name
    /// </summary>
    public string GetWorkspaceName()
    {
        try
        {
            return GetText(_workspaceName);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Check if workspace menu is displayed
    /// </summary>
    public bool IsWorkspaceMenuDisplayed()
    {
        return IsElementDisplayed(_workspaceMenu, 5);
    }

    /// <summary>
    /// Open workspace settings
    /// </summary>
    public WorkspacePage OpenWorkspaceSettings()
    {
        Click(_workspaceSettings);
        return this;
    }

    /// <summary>
    /// Click Add Member button
    /// </summary>
    public WorkspacePage ClickAddMember()
    {
        Click(_addMemberButton);
        return this;
    }

    /// <summary>
    /// Check if members list is visible
    /// </summary>
    public bool IsMembersListVisible()
    {
        return IsElementDisplayed(_membersList, 5);
    }

    /// <summary>
    /// Get count of workspace pages
    /// </summary>
    public int GetWorkspacePagesCount()
    {
        try
        {
            var elements = Driver.FindElements(_workspacePages);
            return elements.Count;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Click create new button
    /// </summary>
    public WorkspacePage ClickCreateNew()
    {
        Click(_createNewButton);
        return this;
    }

    /// <summary>
    /// Check if import button is visible
    /// </summary>
    public bool IsImportButtonVisible()
    {
        return IsElementDisplayed(_importButton, 3);
    }

    /// <summary>
    /// Navigate to trash
    /// </summary>
    public WorkspacePage NavigateToTrash()
    {
        Click(_trashButton);
        return this;
    }

    /// <summary>
    /// Verify workspace page is loaded
    /// </summary>
    public bool IsWorkspacePageLoaded()
    {
        return GetCurrentUrl().Contains("notion.so") &&
               IsElementPresent(_workspaceName);
    }
}