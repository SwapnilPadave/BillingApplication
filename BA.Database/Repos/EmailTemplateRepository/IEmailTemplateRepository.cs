using BA.Database.Infra;
using BA.Dtos.EmailTemplateDto;
using BA.Entities.Email;

namespace BA.Database.Repos.EmailTemplateRepository
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplates>
    {
        Task<GetEmailTemplateDetailsDto> GetEmailTemplateByType(string emailType);
    }
}
