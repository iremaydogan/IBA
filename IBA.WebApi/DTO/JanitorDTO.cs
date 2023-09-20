namespace IBA.WebApi.DTO
{
    public class JanitorDTO
    {
        public int JanitorID { get; set; }
        public string JanitorName { get; set; } = string.Empty;
        public string JanitorSurname { get; set; } = string.Empty;
        public int CountryID { get; set; }
    }
}
