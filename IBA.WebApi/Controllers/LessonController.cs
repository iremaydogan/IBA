using IBA.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using IBA.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IBA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : Controller
    {
        private readonly Context _context;
        public LessonController(Context context)
        {
            _context = context;
        }
        [HttpPost("PostLesson")]
        public ActionResult PostLesson(LessonDTO request)
        {

            Lesson item = new Lesson();
            item.LessonName = request.LessonName;
            item.LessonID = request.LessonID;


            _context.Add(item);
            _context.SaveChanges();
            return Ok(item.LessonID);

        }
        [HttpGet("GetLesson")]
        public ActionResult GetLesson(int id)
        {
            var item = _context.Lessons.Find(id);
            return Ok(item);
        }
        [HttpGet("GetAllLesson")]
        public ActionResult GetAllLesson(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var lesson = _context.Lessons.ToList();
                return Ok(lesson);
            }
            else
            {
                var lesson = _context.Lessons.Where(e => e.LessonName.Contains(name)).ToList();
                return Ok(lesson);
            }

        }
        [HttpDelete("DeleteLesson")]
        public ActionResult DeleteLesson(int id)
        {
            var item = _context.Lessons.Find(id);
            _context.Lessons.Remove(item);
            return Ok();
        }
        [HttpPut("PutLesson")]
        public ActionResult PutLesson(LessonDTO request)
        {
            var item = _context.Lessons.Find(request.LessonID);
            if (item != null)
            {
                item.LessonName = request.LessonName;

            }

            return Ok();
        }
    }
}
