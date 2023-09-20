using IBA.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using IBA.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IBA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JanitorController : Controller
    {
        private readonly Context _context;
        public JanitorController(Context context)
        {
            _context = context;
        }
        [HttpPost("PostJanitor")]
        public IActionResult PostJanitor(JanitorDTO request)
        {

            Janitor item = new Janitor();
            item.JanitorName = request.JanitorName;
            item.JanitorSurname = request.JanitorSurname;
            item.CountryID = request.CountryID;
            _context.Add(item);
            _context.SaveChanges();
            return Ok(item.JanitorID);
        }
        [HttpGet("GetJanitor")]
        public IActionResult GetJanitor(int id)
        {
            var item = _context.Janitors.Find(id);
            return Ok(item);
        }
        [HttpGet("GetAllJanitor")]
        public IActionResult GetAllJanitor(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var jantitor= _context.Janitors.ToList();
                return Ok(jantitor);
            }
            else
            {
                var janitor= _context.Janitors.Where(x=>x.JanitorName.Contains(name)).ToList();
                return Ok(janitor);
            }
        }
        [HttpDelete("DeleteJanitor")]
        public IActionResult DeleteJanitor(int id) 
        {
        var item=_context.Janitors.Find(id);
            _context.Janitors.Remove(item);
            return Ok();
        }
        [HttpPut("PutJanitor")]
        public IActionResult PutJanitor(JanitorDTO request)
        {
            var item = _context.Janitors.Find(request.JanitorID);
            if (item != null)
            {
                item.JanitorName = request.JanitorName;
                item.JanitorSurname = request.JanitorSurname;
                item.CountryID = request.CountryID;

            }
            return Ok();



        }
    }
}
