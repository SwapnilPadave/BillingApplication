using BA.Api.Infra.Requests.NewsPaperRequests;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class NewsPaperValidator : AbstractValidator<AddNewsPaperRequest>
    {
        public NewsPaperValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("Name is required."));
        }
    }
}
