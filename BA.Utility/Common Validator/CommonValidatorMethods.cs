using BA.Utility.Content;

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
        public static bool IsValidDate(string? date)
        {
            if (date != null)
                return DateTime.TryParse(date, out _);
            return true;
        }
        public static string IsBothDatesProvided(string? fromDate, string? toDate)
        {
            if (!CheckStringValue(fromDate) && CheckStringValue(toDate))
                return ContentLoader.ReturnLanguageData("BA603");
            if (CheckStringValue(fromDate) && !CheckStringValue(toDate))
                return ContentLoader.ReturnLanguageData("BA604");
            return ContentLoader.ReturnLanguageData("BA100");
        }
    }
}
