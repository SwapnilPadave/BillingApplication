using BA.Api.Infra.Requests.CustomerRequest;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.BuildingName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1107"));

            RuleFor(x => x.RoomNo)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1108"));

            RuleFor(x => x.AreaName)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1109"));
        }
    }
}
