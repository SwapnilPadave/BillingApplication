using System.ComponentModel.DataAnnotations;

namespace BA.Entities.Infra
{
    public class ExceptionLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime ExceptionDateTime { get; set; } = DateTime.Now;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string? ExceptionType { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerExceptionMessage { get; set; }
    }
}
