using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IBA.WebApi.Model;
using IBA.WebApi.DTO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace IBA.WebApi.Controllers
{
    [Authorize(Roles = "Ogrenci,Admin")]
    [Route("api/[controller]/[action]")]


    public class StudentController : ControllerBase
    {
        private readonly Context _context;
        public StudentController(Context context)
        {
            _context = context;
        }

        [HttpPost("PostStudent")]
        public ActionResult PostStudent(StudentDTO request)
        {
            if (new EmailAddressAttribute().IsValid(request.StudentEmail))
            {
                Student item = new Student();
                item.StudentNumber = request.StudentNumber;
                item.StudentName = request.StudentName;
                item.StudentEmail = request.StudentEmail;//TODO:Email kontrolü yap
                item.StudentSurname = request.StudentSurname;
                item.CountryID = request.CountryID;
                _context.Add(item);
                _context.SaveChanges();

                return Ok(item.StudentID);
            }
            else
            {
                return BadRequest("Email Hatalı!");
            }

        }
        [HttpGet]
        public ActionResult GetStudent(int id)
        {
            var result = _context.Students.FirstOrDefault(x=>x.StudentID == id);
            return Ok(result);
        }
        [HttpGet("GetAllStudent")]
        public ActionResult GetAllStudent(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var student = _context.Students.ToList();
                return Ok(student);
            }
            else
            {
                var student = _context.Students.Where(e => e.StudentName.Contains(name)).ToList();
                return Ok(student);
            }
        }
        [HttpPut]
        public ActionResult PutStudent(Student student, int id)
        {
            var guncelle = _context.Students.Find(id);
            if (guncelle != null)
            {
                guncelle.StudentName = student.StudentName;
                guncelle.StudentSurname = student.StudentSurname;
                guncelle.StudentEmail = student.StudentEmail;
                guncelle.StudentNumber = student.StudentNumber;
                guncelle.Countrys = student.Countrys;
                _context.SaveChanges();
                return Ok();
            }
            return Ok();
        }
        [HttpDelete("DeleteStudent")]
        public ActionResult DeleteStudent(int id)
        {
            var silinecek = _context.Students.Find(id);
            _context.Remove(silinecek);
            _context.SaveChanges();
            return Ok();
        }

    }
}
