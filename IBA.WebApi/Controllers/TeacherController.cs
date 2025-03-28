﻿using IBA.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using IBA.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace IBA.WebApi.Controllers
{
    [Authorize(Roles = "Ogretmen,Admin")]
  
    [Route("api/[controller]/[action]")]
    public class TeacherController : Controller
    {
        private readonly Context _context;

        public TeacherController(Context context)
        {
            _context = context;
        }
        [HttpPost("PostTeacher")]
        public ActionResult PostTeacher([FromBody] TeacherDTO request)
        {
            if (new EmailAddressAttribute().IsValid(request.TeacherEmail))
            {
                Teacher item = new Teacher();

                item.TeacherName = request.TeacherName;
                item.TeacherEmail = request.TeacherEmail;//TODO:Email kontrolü yap
                item.TeacherSurname = request.TeacherSurname;
                item.CountryID = request.CountryID;
                _context.Add(item);
                _context.SaveChanges();
                return Ok(item.TeacherID);

            }
            else
            {
                return BadRequest("Email Hatalı!");
            }
        }

        [HttpGet("GetTeacher")]
        public ActionResult GetTeacher([FromQuery] int id)
        {
            var teach = _context.Teachers.FirstOrDefault(x => x.TeacherID == id);
            return Ok(teach);
        }
        [HttpGet("GetAllTeacher")]
        public ActionResult GetAllTeacher([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var teacher = _context.Teachers.ToList();
                return Ok(teacher);
            }
            else
            {
                var teacher = _context.Teachers.Where(e => e.TeacherName.Contains(name)).ToList();
                return Ok(teacher);
            }

        }

        [HttpDelete("DeleteTeacher")]
        public ActionResult DeleteTeacher([FromBody] int id)
        {
            var silinecek = _context.Teachers.Find(id);
            _context.Remove(silinecek);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("PutTeacher")]
        public ActionResult PutTeacher([FromBody] int id, Teacher teacher)
        {
            var guncelle = _context.Teachers.Find(id);
            if (guncelle != null)
            {
                guncelle.TeacherName = teacher.TeacherName;
                guncelle.TeacherSurname = teacher.TeacherSurname;
                guncelle.TeacherEmail = teacher.TeacherEmail;
                guncelle.TeacherBranch = teacher.TeacherBranch;
                _context.SaveChanges();
                return Ok();
            }
            return Ok();

        }
    }
}

