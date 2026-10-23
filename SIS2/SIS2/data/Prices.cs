using System.Globalization;

namespace SIS2.data
{
    public static class Prices
    {
        /// <summary>Turns "$48.41" or "48.41" into 48.41m (culture-independent).</summary>
        public static decimal Parse(string shown) =>
            decimal.Parse(shown.Replace("$", "").Trim(), NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>Formats a price the way the cart shows it, e.g. 28.3m -> "$28.30".</summary>
        public static string Format(decimal price) =>
            "$" + price.ToString("0.00", CultureInfo.InvariantCulture);
    }
}
