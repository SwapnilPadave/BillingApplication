using BA.Database.Infra;
using BA.Dtos.EmailTemplateDto;
using BA.Entities.Email;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.EmailTemplateRepository
{
    public class EmailTemplateRepository : Repository<EmailTemplates>, IEmailTemplateRepository
    {
        private readonly BAContext _context;
        public EmailTemplateRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GetEmailTemplateDetailsDto> GetEmailTemplateByType(string emailType)
        {
            var data = await (from et in _context.EmailTemplates
                              where et.EmailType.Trim().ToLower() == emailType.Trim().ToLower() && et.IsActive
                              select new GetEmailTemplateDetailsDto
                              {
                                  EmailType = et.EmailType,
                                  EmailSubject = et.EmailSubject,
                                  EmailBody = et.EmailBody,
                                  EmailTableBody = et.EmailTableBody
                              }).FirstOrDefaultAsync();
            return data!;
        }
    }
}
