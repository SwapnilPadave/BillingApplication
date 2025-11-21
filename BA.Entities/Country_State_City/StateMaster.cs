using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Country_State_City
{
    [Table("StateMaster")]
    public class StateMaster
    {
        [Key]
        public int Id { get; set; }
        public string StateName { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
