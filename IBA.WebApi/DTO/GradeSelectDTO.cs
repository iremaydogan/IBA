
namespace IBA.WebApi.DTO
{
    public class GradeSelectDTO
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int ExamGrade { get; set; }
        public string LessonName { get; set; } = string.Empty;
    }

}
