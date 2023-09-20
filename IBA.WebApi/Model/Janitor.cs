using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IBA.WebApi.Model
{
    public class Janitor
    {
        [Key]
        public int JanitorID { get; set; }
        public string JanitorName { get; set; } = string.Empty;
        public string JanitorSurname { get; set; } = string.Empty;


        [ForeignKey("CountryID")]
        public virtual Country Countrys { get; set; }
        public int CountryID { get; set; }
    }
}
