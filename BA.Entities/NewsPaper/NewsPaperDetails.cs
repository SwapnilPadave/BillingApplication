using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.NewsPaper
{
    [Table("NewsPaperDetails")]
    public class NewsPaperDetails : AuditProperties
    {
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
