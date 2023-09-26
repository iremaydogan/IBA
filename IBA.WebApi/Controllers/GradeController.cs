using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using IBA.WebApi.Model;
using IBA.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace IBA.WebApi.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]/[action]")]
    public class GradeController : Controller
    {
        private readonly Context _context;

        public GradeController(Context context)
        {
            _context = context;
        }
        [HttpPost("PostGrade")]
        public ActionResult PostGrade(GradeDTO request)
        {
            var student = _context.Students.Find(request.StudentID);
            if (request.StudentGrade > 100)
            {
                return BadRequest("Not Hatalı yeniden giriniz:!");
            }


            if (student == null)
            {
                return StatusCode(500, $"Merhaba. {request.StudentID} Nolu Öğrenci Bulunamadı!");
            }
            Grade item = new Grade();
            item.LessonID = request.LessonID;
            item.StudentID = request.StudentID;
            item.StudentGrade = request.StudentGrade;
            _context.Add(item);
            _context.SaveChanges();
            return Ok(item.GradeID);
        }
        [HttpGet("GetGrade")]
        public ActionResult GetGrade(int id)
        {
            var asd = _context.Grades.Include(x => x.Lessons).Include(x => x.Students).Where(x => x.GradeID == id).FirstOrDefault();

            if (asd != null)
            {
                return Ok(asd);
            }
            else
            {
                return BadRequest("Kayıt Bulunamadı!");
            }
        }
        [HttpGet("GetAllGrade")]
        public ActionResult GetAllGrade(int? not1, int? not2)
        {
            var grade = new List<Grade>();
            if (not1 != null && not2 != null)
            {

                grade = _context.Grades.Where(x => x.StudentGrade >= not1 && x.StudentGrade <= not2).ToList();
            }
            else
            {
                grade = _context.Grades.ToList();
            }

            if (grade.Count > 0)
            {
                return Ok(grade);
            }
            else
            {
                return BadRequest("Kayıt Bulunamadı!");
            }
        }

        [HttpDelete("DeleteGrade")]
        public ActionResult DeleteGrade(int id)
        {
            var grades = _context.Grades.Find(id);

            _context.Grades.Remove(grades);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("PutGrade")]
        public ActionResult PutGrade(GradeDTO request, int id)
        {

            var item = _context.Grades.Find(id);
            if (item != null)
            {
                item.StudentID = request.StudentID;
                item.StudentGrade = request.StudentGrade;
                item.LessonID= request.LessonID;
                _context.SaveChanges();
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
