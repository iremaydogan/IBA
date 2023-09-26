using System.ComponentModel.DataAnnotations;

namespace IBA.WebApi.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;    
        public  DateTime DateTime {  get; set; } 
    }
}
