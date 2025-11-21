using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Country_State_City
{
    [Table("CountryMaster")]
    public class CountryMaster
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}
