using System.ComponentModel.DataAnnotations;
namespace IBA.WebApi.Model
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
