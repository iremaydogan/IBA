using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
namespace IBA.WebApi.Model
{
    public class Lesson
    {
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;

    }
}
