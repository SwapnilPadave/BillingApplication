using BA.Api.Infra.Requests.EmployeeRequest;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using BA.Utility.Regex;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class AddEmployeeValidator : AbstractValidator<AddEmployeeRequest>
    {
        public AddEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1401"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1410"));

            RuleFor(x => x.MiddleName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1402"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1411"));

            RuleFor(x => x.LastName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1403"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1412"));

            RuleFor(x => x.Email)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1404"))
                .EmailAddress()
                .WithMessage(ContentLoader.ReturnLanguageData("BA1413"));

            RuleFor(x => x.MobileNumber)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1405"))
                .Must(RegexValidatorHelper.IsValidPhoneNumber)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1414"));

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1407"));

            RuleFor(x => x.DateOfJoining)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1408"));

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1409"));

            RuleFor(x => x.Address)
                //.Must(CommonValidatorMethods.CheckStringValue)
                //.WithMessage(ContentLoader.ReturnLanguageData("BA1406"))
                .Must(RegexValidatorHelper.IsValidAddress)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1415"));
        }
    }

    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1401"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1410"));

            RuleFor(x => x.MiddleName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1402"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1411"));

            RuleFor(x => x.LastName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1403"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1412"));

            RuleFor(x => x.Email)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1404"))
                .EmailAddress()
                .WithMessage(ContentLoader.ReturnLanguageData("BA1413"));

            RuleFor(x => x.MobileNumber)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1405"))
                .Must(RegexValidatorHelper.IsValidPhoneNumber)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1414"));

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1407"));

            RuleFor(x => x.DateOfJoining)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1408"));

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1409"));

            RuleFor(x => x.Address)
                //.Must(CommonValidatorMethods.CheckStringValue)
                //.WithMessage(ContentLoader.ReturnLanguageData("BA1406"))
                .Must(RegexValidatorHelper.IsValidAddress)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1415"));
        }
    }
}
