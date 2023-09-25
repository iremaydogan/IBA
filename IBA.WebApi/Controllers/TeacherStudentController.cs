using IBA.WebApi.DTO;
using IBA.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeacherStudentController : Controller
    {
        private readonly Context _context;

        public TeacherStudentController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult PostStudentTeacher(TeacherStudentDTO request)
        {
            var item = new TeacherStudent();
            item.StudentID = request.StudentID;
            item.TeacherID = request.TeacherID;
            _context.Add(item);
            _context.SaveChanges();
            return Ok(item.TeacherStudentID);
        }

        [HttpGet]
        public ActionResult GetStudentTeacher(int id)
        {

            var list = (from A in _context.TeacherStudents
                        join B in _context.Teachers on A.TeacherID equals B.TeacherID
                        join C in _context.Students on A.StudentID equals C.StudentID
                        where A.TeacherStudentID == id
                        select new TeacherStudentByIdDTO
                        {
                            StudentID = A.StudentID,
                            StudentName = C.StudentName,
                            TeacherID = A.TeacherID,
                            TeacherName = B.TeacherName
                        }).ToList();
            return Ok(list);
        }

        [HttpDelete]
        public ActionResult DeleteStudentTeacher(int id)
        {
            var silinecek = _context.TeacherStudents.Find(id);
            _context.Remove(silinecek);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public ActionResult PutStudentTeacher(int id, TeacherStudent request)
        {
            var guncelle = _context.TeacherStudents.Find(id);
            if (guncelle != null)
            {
                guncelle.TeacherStudentID = request.TeacherStudentID;
                guncelle.StudentID = request.StudentID;
                _context.SaveChanges();
                return Ok();
            }
            return Ok();

        }
    }
}
