using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
namespace IBA.WebApi.Model
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentSurname { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public int StudentNumber { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country Countrys { get; set; }
        public int CountryID { get; set; }
    }
}
