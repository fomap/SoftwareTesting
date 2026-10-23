namespace SIS2.data
{
    public sealed record Customer(
        string FirstName, string LastName, string DateOfBirth,
        string Country, string PostalCode, string HouseNumber,
        string Street, string City, string State,
        string Phone, string Email, string Password)
    {
        public string FullName => $"{FirstName} {LastName}";
    }

    /// <summary>
    /// Fresh test users. Every call returns a new, never-used email
    /// (timestamp + GUID), so runs and parallel tests never collide.
    /// The seeded accounts (customer@ / customer2@practicesoftwaretesting.com, welcome01)
    /// are not used: other people's failed logins lock them, and a password reset
    /// would change them for everyone.
    /// </summary>
    public static class UserFactory
    {
        public static Customer NewCustomer() => new(
            FirstName: "Dana",
            LastName: "Tester",
            DateOfBirth: "1995-04-12",
            Country: "KZ",
            PostalCode: "050000",
            HouseNumber: "12",
            Street: "Abay Avenue",
            City: "Almaty",
            State: "Almaty",
            Phone: "77011234567",
            Email: $"sis2.{Unique()}@example.com",
            Password: $"Sis2#{Guid.NewGuid():N}"[..17] + "Aa9"); // upper, lower, digit, symbol

        public static string UnregisteredEmail() => $"sis2.nobody.{Unique()}@example.com";

        public static string WrongPassword() => $"Wrong#{Guid.NewGuid():N}"[..18] + "Aa9";

        private static string Unique() => $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.{Guid.NewGuid():N}"[..30];
    }
}
