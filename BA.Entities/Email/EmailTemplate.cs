using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Email
{
    [Table("EmailTemplates")]
    public class EmailTemplates : AuditProperties
    {
        public string EmailType { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public string EmailTableBody { get; set; } = string.Empty;

    }
}
