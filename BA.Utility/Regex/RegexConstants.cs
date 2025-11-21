namespace BA.Utility.Regex
{
    public static class RegexConstants
    {
        public const string ADDRESS_REGEX = @"^[a-zA-Z0-9\s,.'-]{3,}$";
        public const string NAME_REGEX = @"^[a-zA-Z\s'`-]{2,}$";
        public const string PHONE_NUMBER_REGEX = @"^(?:\+91|0)?[6-9]\d{9}$";
    }
}
