using Microsoft.AspNetCore.Mvc;
using IBA.WebApi.Model;
using Microsoft.AspNetCore.Http;
namespace IBA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller//x
    {     
            private readonly Context _context;
            public CountryController(Context context)
            {
                _context = context;
            }
            [HttpPost("PostCountry")]
            public IActionResult PostCountry(Country country)
            {
                _context.Add(country);
                _context.SaveChanges();
                return Ok(country.CountryID);
            }
            [HttpGet("GetCountry")]
            public IActionResult GetCountry(int id)
            {
                var count = _context.Countrys.Find(id);
                return Ok(count);

            }
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
