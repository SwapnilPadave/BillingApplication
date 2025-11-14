using BA.Api.Infra.Requests.UserRequests;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators.UserValidations
{
    public class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Name)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1001"))
                .Length(3, 200)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1002"));

            RuleFor(x => x.Email)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1003"))
                .EmailAddress()
                .WithMessage(ContentLoader.ReturnLanguageData("BA1004"));

            RuleFor(x => x.MobileNumber)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1005"))
                .Matches(@"^\d{10}$")
                .WithMessage(ContentLoader.ReturnLanguageData("BA1006"));

            RuleFor(x => x.Age)
                .Must(CommonValidatorMethods.CheckNumberValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1009"));
        }
    }
}
