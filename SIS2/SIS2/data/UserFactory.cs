namespace SIS2.data
{
    /// <summary>
    /// Builds test users. Tests ask for a user here and never hard-code one.
    /// </summary>
    public static class UserFactory
    {
        private static readonly string[] FirstNames = { "Aruzhan", "Dias", "Madina", "Timur", "Aigerim", "Nurlan", "Dana", "Arman" };
        private static readonly string[] LastNames = { "Tester", "Qa", "Automation", "Checker", "Verifier", "Runner" };

        /// <summary>
        /// A brand-new customer that has never been registered.
        /// The email gets a millisecond UTC timestamp plus a GUID fragment, so two calls
        /// never collide: not between runs, not between tests running in parallel,
        /// and not with other people using the same public site.
        /// </summary>
        public static CustomerData NewCustomer()
        {
            var unique = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.{Guid.NewGuid():N}"[..30];
            var rnd = Random.Shared; // thread-safe

            return new CustomerData(
                FirstName: Pick(FirstNames),
                LastName: Pick(LastNames),
                DateOfBirth: DateTime.UtcNow.Date.AddYears(-rnd.Next(20, 60)).AddDays(-rnd.Next(0, 365)).ToString("yyyy-MM-dd"),
                CountryCode: "KZ",
                PostalCode: "050000",
                HouseNumber: rnd.Next(1, 200).ToString(),
                Street: "Abay Avenue",
                City: "Almaty",
                State: "Almaty",
                Phone: "7701" + rnd.Next(1000000, 9999999),
                Email: $"sis2.{unique}@example.com",
                // Meets the site's rules: 8+ chars, upper + lower, digit, special symbol.
                Password: $"Sis2#{Guid.NewGuid():N}"[..17] + "Aa9");
        }

        private static string Pick(string[] values) => values[Random.Shared.Next(values.Length)];
    }
}
