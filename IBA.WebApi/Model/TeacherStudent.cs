using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IBA.WebApi.Model
{
    public class TeacherStudent
    {
        [Key]
        public int TeacherStudentID { get; set; }

        [ForeignKey("TeacherID")]
        public virtual Teacher Teachers { get; set; }
        public int TeacherID { get; set; }


        [ForeignKey("StudentID")]
        public virtual Student Students { get; set; }
        public int StudentID { get; set; }
    }
}
