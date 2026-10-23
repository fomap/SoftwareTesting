namespace SIS2.data
{
    /// <summary>
    /// Accounts that already exist on practicesoftwaretesting.com.
    /// Use these for tests that need an existing account (login, checkout, ...).
    /// Read-only use only: other people share them, so never change their password or profile.
    /// </summary>
    public static class SeededUsers
    {
        public static readonly (string Email, string Password) Customer =
            ("customer@practicesoftwaretesting.com", "welcome01");

        public static readonly (string Email, string Password) Customer2 =
            ("customer2@practicesoftwaretesting.com", "welcome01");
    }
}
