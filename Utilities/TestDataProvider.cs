namespace QA_Project.Utilities;

/// <summary>
/// Provides test data for various test scenarios
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// Get invalid email test data
    /// </summary>
    public static IEnumerable<object[]> GetInvalidEmails()
    {
        yield return new object[] { "" }; // Empty
        yield return new object[] { "notanemail" }; // No @ symbol
        yield return new object[] { "@example.com" }; // Missing local part
        yield return new object[] { "test@" }; // Missing domain
        yield return new object[] { "test@.com" }; // Invalid domain
        yield return new object[] { "test space@example.com" }; // Contains space
    }

    /// <summary>
    /// Get invalid password test data
    /// </summary>
    public static IEnumerable<object[]> GetInvalidPasswords()
    {
        yield return new object[] { "" }; // Empty
        yield return new object[] { "123" }; // Too short
        yield return new object[] { "   " }; // Only spaces
    }

    /// <summary>
    /// Get valid email formats for testing
    /// </summary>
    public static IEnumerable<object[]> GetValidEmailFormats()
    {
        yield return new object[] { "test@example.com" };
        yield return new object[] { "user.name@example.com" };
        yield return new object[] { "user+tag@example.co.uk" };
        yield return new object[] { "test123@test-domain.com" };
    }

    /// <summary>
    /// Get browser types for cross-browser testing
    /// </summary>
    public static IEnumerable<object[]> GetBrowserTypes()
    {
        yield return new object[] { "Chrome" };
        yield return new object[] { "Edge" };
    }

    /// <summary>
    /// Get test user credentials
    /// </summary>
    public static class TestUsers
    {
        public static (string Email, string Password) ValidUser =>
            ("valid@example.com", "ValidPassword123!");

        public static (string Email, string Password) InvalidUser =>
            ("invalid@example.com", "WrongPassword123!");

        public static (string Email, string Password) EmptyCredentials =>
            ("", "");
    }
}