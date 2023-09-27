using Microsoft.AspNetCore.Mvc;
using IBA.WebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using IBA.WebApi.DTO;
using Microsoft.Extensions.Caching.Memory;
using IBA.WebApi.Installers;

namespace IBA.WebApi.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]

    public class CountryController : Controller//x
    {
        private readonly Context _context;
        protected internal IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        public CountryController(Context context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache= memoryCache;
        }
        [HttpPost("PostCountry")]
        public IActionResult PostCountry(Country country)
        {

            _context.Add(country);
            _context.SaveChanges();
            return Ok(country.CountryID);
        }

        [AllowAnonymous]
        [Cached(100)]
        [HttpGet("GetCountry")]
        public IActionResult GetCountry(int id)
        {
            var y = HttpContext.User.Identity?.Name;
            var x = HttpContext.Request.Headers.Host.ToString();

            var count = _context.Countrys.FirstOrDefault(x => x.CountryID == id);
            return Ok(count);

        }

        [AllowAnonymous]
        [Cached(50)]
        [HttpGet("GetAllCountry")]
        public IActionResult GetAllCountry(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var countrys = _context.Countrys.ToList();
                return Ok(countrys);
            }
            else
            {
                var countrys = _context.Countrys.Where(e => e.Name.Contains(name)).ToList();
                return Ok(countrys);
            }


        }

        [HttpDelete("DeleteCountry")]
        public IActionResult DeleteCountry(int id)
        {
            var sil = _context.Countrys.Find(id);
            _context.Countrys.Remove(sil);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("PutCountry")]
        public IActionResult PutCountry(Country country, int id)
        {
            var guncelle = _context.Countrys.Find(id);
            if (guncelle != null)
            {
                guncelle.Name = country.Name;
                guncelle.Code = country.Code;
                _context.SaveChanges();

            }

            return Ok(guncelle);

        }

    }
}
