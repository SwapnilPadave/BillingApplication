using BA.Api.Infra.Requests.UserRequests;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
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

            RuleFor(x => x.Address)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1007"))
                .Length(10, 100)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1008"));

            RuleFor(x => x.Age)
                .Must(CommonValidatorMethods.CheckNumberValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1009"));
        }
    }
}
