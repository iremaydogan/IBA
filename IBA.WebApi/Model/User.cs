using System.ComponentModel.DataAnnotations;

namespace IBA.WebApi.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }  
        public string? UserPassword { get; set; }    
        public  DateTime DateTime {  get; set; } 
        public string? UserRole { get; set; }
    }
}
