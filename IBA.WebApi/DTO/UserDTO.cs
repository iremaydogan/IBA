using System.Text.Json.Serialization;

namespace IBA.WebApi.DTO
{
    public class UserDTO
    {
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public Roles Role { get; set; }

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        Admin = 1,
        Ogretmen = 2,
        Ogrenci = 3
    }
}
