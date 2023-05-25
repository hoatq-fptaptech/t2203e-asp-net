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
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2203E_API.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AutheticationController : ControllerBase
    {
        private readonly T2203eApiContext _context;

        public AutheticationController(T2203eApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(UserRegister user)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(new Entities.User { Email=user.Email,Name=user.Name,Password=hashed });
            _context.SaveChanges();
            return Ok(new UserData { Name=user.Name,Email=user.Email});
        }

        //[HttpPost]
        //[Route("/login")]
        //public IActionResult Login()
        //{
            //bool verified = BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);
        //}
    }
}

