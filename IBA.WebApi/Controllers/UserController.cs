using IBA.WebApi.DTO;
using IBA.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;

namespace IBA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly Context _context;
        public UserController(Context context)
        {
            _context = context;
        }
        [HttpPost("PostUser")]
        [AllowAnonymous]
        public IActionResult PostCountry(UserDTO request)
        { 
            var kullanici = _context.Users.FirstOrDefault(k => k.UserName == request.UserName);
            if (kullanici == null)
            {
                DateTime girisTarihi = DateTime.Now;
                User item = new User();
                item.UserName = request.UserName;
                item.UserPassword = request.UserPass;
                item.DateTime = girisTarihi;
                _context.Add(item);
                _context.SaveChanges();
                return Ok(item.UserId);
            }
            else
            {
                return BadRequest("Bu kullanıcı adı daha önce alınmıştır.");
            }


        }
    }
}
