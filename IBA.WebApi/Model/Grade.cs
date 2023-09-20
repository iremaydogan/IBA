using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IBA.WebApi.Model
{
    public class Grade
    {
        [Key]
        public int GradeID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Students { get; set; }
        public int StudentID { get; set; }

        public int StudentGrade { get; set; }


        [ForeignKey("LessonID")]
        public virtual Lesson Lessons { get; set; }
        public int LessonID { get; set; }
    }
}
