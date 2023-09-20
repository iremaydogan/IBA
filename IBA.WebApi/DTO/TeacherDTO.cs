namespace IBA.WebApi.DTO
{
    public class TeacherDTO
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherSurname { get; set; } = string.Empty;
        public string TeacherEmail { get; set; } = string.Empty;
        public string TeacherBranch { get; set; } = string.Empty;
        public int CountryID { get; set; }
    }
}
