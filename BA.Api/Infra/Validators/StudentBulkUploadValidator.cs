using BA.Dtos.StudentDto;
using BA.Utility.Common_Validator;
using BA.Utility.Content;
using BA.Utility.Regex;
using FluentValidation;

namespace BA.Api.Infra.Validators
{
    public class StudentBulkUploadValidator : AbstractValidator<GetStudentsDetailsForBulkUploadDto>
    {
        public StudentBulkUploadValidator()
        {
            RuleFor(x => x.Name)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1302"))
                .Must(RegexValidatorHelper.IsValidName)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1305"));

            RuleFor(x => x.Email)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1303"))
                .EmailAddress()
                .WithMessage(ContentLoader.ReturnLanguageData("BA1004"));

            RuleFor(x => x.Address)
                .Must(CommonValidatorMethods.CheckStringValue)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1304"))
                .Must(RegexValidatorHelper.IsValidAddress)
                .WithMessage(ContentLoader.ReturnLanguageData("BA1306"));
        }
    }
}
