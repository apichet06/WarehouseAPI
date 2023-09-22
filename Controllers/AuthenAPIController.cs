using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Warehouse_API.Data;
using Warehouse_API.Models;
using Warehouse_API.Models.Dto;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly ResponseDto _response;
        private readonly MessageDto _message;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthenAPIController(AppDBContext db , IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _response = new ResponseDto();
            _message =  new MessageDto();
            _mapper = mapper;
            _configuration = configuration;
        }


      
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Authen(Users user)
        {
            try
            {

                var  users = await _db.Users.SingleOrDefaultAsync(e=>e.Username == user.Username && e.Password == HashPassword(user.Password!));


                if(users != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Username!)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]!));
                    var singIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["jwt:Issuer"],
                        _configuration["jwt:Audience"], 
                    claims, expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: singIn);
                    return Ok(new
                    {
                        Message = "ล็อกอินสำเร็จ!",
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        resulte = users
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "ชื่อหรือรหัสผ่านของคุณไม่ถูกต้อง!"
                    });
                }

            }
            catch (Exception ex)
            {
               return BadRequest(new
                {
                    Message = "เกิดข้อผิดพลาด : " + ex.Message
                });

            }

         
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }


    }
}
