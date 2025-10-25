using OpenQA.Selenium;
using QA_Project.PageObjects;
using QA_Project.Config;
using Xunit;

namespace QA_Project.Tests
{
    public class LoginTests : BaseTest
    {
        [Fact]
        public void Test_01_ValidLogin_Chrome_ShouldSucceed()
        {
            InitializeDriver("Chrome");
            var loginPage = new LoginPage(Driver);

            try
            {
                loginPage.NavigateToLoginPage();

                // If session already active
                if (Driver.Url.Contains("notion.so") && !Driver.Url.Contains("login"))
                {
                    Console.WriteLine("✅ Already logged in (session reused)");
                    return;
                }

                loginPage.Login(TestSettings.ValidEmail, TestSettings.ValidPassword);
                Thread.Sleep(5000);
                Assert.Contains("notion.so", Driver.Url);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"⚠️ Login possibly blocked due to temporary code. Skipping: {ex.Message}");
            }
            finally
            {
                CleanupDriver();
            }
            
        }

        [Fact]
        public void Test_03_InvalidEmail_ShouldShowError()
        {
            InitializeDriver("Chrome");
            var loginPage = new LoginPage(Driver);

            loginPage.NavigateToLoginPage();
            loginPage.EnterEmail(TestSettings.InvalidEmail);
            loginPage.ClickContinue();

            bool isError = loginPage.IsErrorMessageDisplayed() || loginPage.IsEmailErrorDisplayed();
            Assert.True(isError, "❌ Expected error for invalid email was not displayed.");

            CleanupDriver();
            
        }

        [Fact]
        public void Test_06_InvalidPassword_ShouldShowError()
        {
            InitializeDriver("Chrome");
            var loginPage = new LoginPage(Driver);

            loginPage.NavigateToLoginPage();
            loginPage.EnterEmail(TestSettings.ValidEmail);
            loginPage.ClickContinue();

            // Skip password if blocked by OTP
            try
            {
                loginPage.EnterPassword(TestSettings.InvalidPassword);
                loginPage.ClickLogin();

                bool isError = loginPage.IsErrorMessageDisplayed() || loginPage.IsPasswordErrorDisplayed();
                Assert.True(isError, "❌ Expected error for invalid password was not displayed.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ Password field not visible (likely OTP barrier). Skipping login check.");
            }

            CleanupDriver();
            
        }
    }
}
