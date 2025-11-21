using BA.Api.Infra.Requests.CommonRequest;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class ReportRequestValidator : AbstractValidator<CommonDateFilterRequest>
    {
        public ReportRequestValidator()
        {
            RuleFor(x => x.FromDate)
                .Must(CommonValidatorMethods.IsValidDate)
                .WithMessage(ContentLoader.ReturnLanguageData("BA605"));

            RuleFor(x => x.ToDate)
                .Must(CommonValidatorMethods.IsValidDate)
                .WithMessage(ContentLoader.ReturnLanguageData("BA606"));
        }
    }
}
