using BA.Api.Infra.Requests.NewsPaperRequests;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class AddNewsPaperValidator : AbstractValidator<AddNewsPaperRequest>
    {
        public AddNewsPaperValidator()
        {
            RuleFor(x => x.Name)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA501"));

            RuleFor(x => x.Language)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA502"));
        }
    }
}
