using BA.Api.Infra.Requests.LoginRequest;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserId)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA201"));

            RuleFor(x => x.Password)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA202"));
        }
    }
}
