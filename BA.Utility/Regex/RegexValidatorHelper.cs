namespace BA.Utility.Regex
{
    public static class RegexValidatorHelper
    {
        public static bool IsValidName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, RegexConstants.NAME_REGEX);
        }
        public static bool IsValidAddress(string address)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(address, RegexConstants.ADDRESS_REGEX);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, RegexConstants.PHONE_NUMBER_REGEX);
        }
    }
}
