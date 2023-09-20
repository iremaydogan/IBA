using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IBA.WebApi.Model
{
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherSurname { get; set; } = string.Empty;
        public string TeacherEmail { get; set; } = string.Empty;
        public string TeacherBranch { get; set; } = string.Empty;

        [ForeignKey("CountryID")]
        public virtual Country Countrys { get; set; }
        public int CountryID { get; set; }
    }
}
