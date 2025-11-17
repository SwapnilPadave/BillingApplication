using FluentValidation;

namespace BA.Utility.ExcelHelper
{
    public static class ValidationHelper
    {
        public static (bool hasError, int errorRowCount) ValidateList<T>(IEnumerable<T> items, AbstractValidator<T> validator)
        {
            bool hasErrors = false;
            int errorRowCount = 0;

            foreach (var item in items)
            {
                var result = validator.Validate(item);

                if (!result.IsValid)
                {
                    hasErrors = true;
                    errorRowCount++;

                    var errorListProp = item.GetType().GetProperty("Errors");
                    if (errorListProp != null)
                    {
                        var list = errorListProp.GetValue(item) as IList<string>;
                        foreach (var err in result.Errors)
                        {
                            list?.Add(err.ErrorMessage);
                        }
                    }
                }
            }

            return (hasErrors, errorRowCount);
        }
    }

}
