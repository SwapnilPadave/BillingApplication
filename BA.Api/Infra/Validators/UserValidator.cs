using BA.Api.Infra.Requests.UserRequests;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators.UserValidations
{
    public class UserValidator : AbstractValidator<AddUserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ContentLoader.ReturnLanguageData("BA1003"))
                .Length(3, 50).WithMessage(ContentLoader.ReturnLanguageData("BA1004"));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ContentLoader.ReturnLanguageData("BA1005"))
                .EmailAddress().WithMessage(ContentLoader.ReturnLanguageData("BA1006"));

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage(ContentLoader.ReturnLanguageData("BA1007"))
                .Matches(@"^\d{10}$").WithMessage(ContentLoader.ReturnLanguageData("BA1008"));

            RuleFor(x => x.Address)
                .Length(10, 100).WithMessage(ContentLoader.ReturnLanguageData("BA1009"));
        }
    }
}
