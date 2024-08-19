using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IronDoneAPI.Middelwares.Attack;
using IronDoneAPI.Models;
using IronDoneAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IronDoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private string GenerateToken(string userIP)
        {
            // token handler can create token
            var tokenHandler = new JwtSecurityTokenHandler();

            string secretKey = "1234dfnjdfmktybsdkbjsdiobjspodtrerjsdfg5678"; //TODO: remove this from code
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            // token descriptor describe HOW to create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // things to include in the token
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userIP), }),
                // expiration time of the token
                Expires = DateTime.UtcNow.AddMinutes(1),
                // the secret key of the token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // converting the token to string
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        // api/Auth/Register
        [HttpPost("Register")]
        public IActionResult RegisterUser(Person person)
        {
            Guid newPersonId = Guid.NewGuid();
            if (person == null)
            {
                return BadRequest();
            }

            person.Id = newPersonId;
            DbService.PersonesList.Add(person);
            return StatusCode(
                StatusCodes.Status201Created,
                new { messege = "User registered successfully." }
            );
        }

        // api/Auth
        [HttpPost]
        public IActionResult Login(Person user)
        {
            if (user.UserName == "admin" && user.Password == "123456")
            {
                string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return StatusCode(200, new { token = GenerateToken(userIP) });
            }
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                new { error = "invalid credentials" }
            );
        }
    }
}
