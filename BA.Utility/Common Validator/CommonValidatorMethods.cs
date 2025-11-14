namespace BA.Utility.Common_Validator
{
    public static class CommonValidatorMethods
    {
        public static bool CheckStringValue(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool CheckNumberValue(int value)
        {
            return value > 0;
        }
    }
}
