namespace BA.Utility.Regex
{
    public static class RegexConstants
    {
        public const string ADDRESS_REGEX = @"^[a-zA-Z0-9\s,.'-]{3,}$";
        public const string NAME_REGEX = @"^[a-zA-Z\s'`-]{2,}$";
    }
}
