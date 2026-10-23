namespace SIS2.data
{
    /// <summary>Everything the registration form needs for one customer.</summary>
    public sealed record CustomerData(
        string FirstName,
        string LastName,
        string DateOfBirth,   // yyyy-MM-dd, the format the form expects
        string CountryCode,   // value of the <option> in the country select
        string PostalCode,
        string HouseNumber,
        string Street,
        string City,
        string State,
        string Phone,
        string Email,
        string Password)
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}
