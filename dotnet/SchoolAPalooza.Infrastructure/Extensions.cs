namespace SchoolAPalooza.Infrastructure
{
    public static class Extensions
    {
        public static string FormatWith(this string formatString, params object[] @params)
        {
            return string.Format(formatString, @params);
        }

        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool AsBool(this string valueAsString)
        {
            bool result;
            bool.TryParse(valueAsString, out result);

            return result;
        }
    }
}
