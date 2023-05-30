using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2203E_API.Entities;
using System.Security.Cryptography;
using T2203E_API.Dtos;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace T2203E_API.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AutheticationController : ControllerBase
    {
        private readonly T2203eApiContext _context;
        private readonly IConfiguration _config;

        public AutheticationController(T2203eApiContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister user)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var u = new Entities.User { Email = user.Email, Name = user.Name, Password = hashed };
            _context.Users.Add(u);
            _context.SaveChanges();
            return Ok(new UserData { Name=user.Name,Email=user.Email,Token= GenerateJWT(u)});
        }

        private String GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email)
            };
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = _context.Users.Where(u => u.Email.Equals(userLogin.Email))
                .First();
            if (user == null)
                return Unauthorized();
            bool verified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
            if(!verified)
                return Unauthorized();

            return Ok(new UserData {Id=user.Id, Name = user.Name, Email = user.Email, Token = GenerateJWT(user) });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                var Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user =  new UserData
                {
                    Id = Convert.ToInt32(Id),
                    Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(user);
            }
            return Unauthorized();
        }
    }
}

