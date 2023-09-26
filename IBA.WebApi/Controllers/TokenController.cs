using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using IBA.WebApi.DTO;
using IBA.WebApi.Model;

namespace IBA.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {

        //private readonly IConfiguration _config;
        private readonly Context _context;

        private readonly IConfiguration _config;

        public TokenController(IConfiguration config, Context context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("PostToken")]
        public async Task<IActionResult> PostToken(TokenDTO request)
        {
            if (request == null)
            {
                return BadRequest("Kullanıcı Bilgileri Boş Olamaz!");
            }
            var item = _context.Users.FirstOrDefault(x => x.UserName == request.UserName && x.UserPassword == request.UserPass);
            if (item == null)
            {
                return BadRequest("Kullanıcı Bulunamadı!");
            }
            else
            {
                string token = GenerateToken(item.UserName,item.UserRole);
                return Ok(token);
            }
            //if (pwd != "123") return Unauthorized("Şifre geçersiz.");
            //string token = GenerateToken();
            //return Ok(token);
        }

        private string GenerateToken(string userName,string userRole)
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(_config["JwtTokenOptions:PrivateKey"]), out _);


            var claims = new List<Claim>()
    {
             new Claim("name", userName)
    };

            var roleClaims = new List<Claim>()
    {
             new Claim("role", userRole)
    };

            claims.AddRange(roleClaims);

            var handler = new JsonWebTokenHandler();
            var now = DateTime.UtcNow;
            var tokenData = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _config["JwtTokenOptions:Issuer"],
                Audience = _config["JwtTokenOptions:Audience"],
                IssuedAt = now,
                NotBefore = now,
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(double.Parse(_config["JwtTokenOptions:Expiration"])),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSsaPssSha256)
            });

            return tokenData;
        }
    }
}
