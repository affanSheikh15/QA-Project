using OpenQA.Selenium;
using QA_Project.Config;
using QA_Project.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace QA_Project.PageObjects
{
    /// <summary>
    /// Page Object for Notion Login Page
    /// </summary>
    public class LoginPage : BasePage
    {
        // Locators (unchanged)
        private readonly By _emailInput = By.CssSelector("input[type='email'], input[placeholder*='email' i]");
        private readonly By _passwordInput = By.CssSelector("input[id^='notion-password-input']");
        private readonly By _continueButton = By.XPath("//div[@role='button' and normalize-space()='Continue']");
        private readonly By _loginButton = By.XPath("//div[@role='button' and normalize-space()='Continue with password']");
        private readonly By _errorMessage = By.CssSelector("div[class*='error'], div[class*='Error'], [role='alert']");
        private readonly By _emailError = By.XPath("//*[contains(text(), 'email') or contains(text(), 'Email')]//ancestor::div[contains(@class, 'error')]");
        private readonly By _passwordError = By.XPath("//*[contains(text(), 'password') or contains(text(), 'Password')]//ancestor::div[contains(@class, 'error')]");
        private readonly By _pageTitle = By.TagName("h1");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public LoginPage NavigateToLoginPage()
        {
            NavigateTo(TestSettings.NotionLoginUrl);
            WaitForPageLoad();
            return this;
        }

        public LoginPage EnterEmail(string email)
        {
            EnterText(_emailInput, email);
            return this;
        }

        public LoginPage ClickContinue()
        {
            WaitHelper.WaitForElementClickable(Driver, _continueButton, 15);
            Click(_continueButton);
            Console.WriteLine("✅ Clicked 'Continue' after entering email.");
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            WaitHelper.WaitForElementVisible(Driver, _passwordInput, 20);
            EnterText(_passwordInput, password);
            Console.WriteLine("✅ Password field located and filled successfully.");
            return this;
        }

        public LoginPage ClickLogin()
        {
            WaitHelper.WaitForElementClickable(Driver, _loginButton, 15);
            Click(_loginButton);
            return this;
        }

        public void Login(string email, string password)
        {
            EnterEmail(email);
            ClickContinue();
            Thread.Sleep(2000);
            EnterPassword(password);
            ClickLogin();

            if (TestSettings.SaveCookiesAfterLogin)
            {
                try
                {
                    Thread.Sleep(5000); // wait for dashboard to load
                    var cookies = Driver.Manage().Cookies.AllCookies.ToList();
                    var cookieList = cookies.Select(c => new
                    {
                        c.Name,
                        c.Value,
                        c.Domain,
                        c.Path,
                        Expiry = c.Expiry?.ToString("o")
                    }).ToList();

                    var json = JsonSerializer.Serialize(cookieList, TestSettings.JsonOptions);

                    // Ensure directory exists
                    var dir = Path.GetDirectoryName(TestSettings.CookieFilePath);
                    if (!string.IsNullOrEmpty(dir))
                        Directory.CreateDirectory(dir);

                    File.WriteAllText(TestSettings.CookieFilePath, json);
                    Console.WriteLine("🍪 Session cookies saved successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to save cookies: {ex.Message}");
                }
            }
        }

        public bool IsErrorMessageDisplayed() => IsElementDisplayed(_errorMessage, 5);
        public string GetErrorMessageText() => IsErrorMessageDisplayed() ? GetText(_errorMessage) : string.Empty;
        public bool IsEmailErrorDisplayed() => IsElementDisplayed(_emailError, 3);
        public bool IsPasswordErrorDisplayed() => IsElementDisplayed(_passwordError, 3);

        public bool IsLoginPageLoaded()
        {
            return GetCurrentUrl().Contains("notion.so/login") ||
                   IsElementPresent(_emailInput);
        }

        public string GetLoginPageTitle()
        {
            try
            {
                return GetText(_pageTitle);
            }
            catch
            {
                return GetPageTitle();
            }
        }
    }
}
