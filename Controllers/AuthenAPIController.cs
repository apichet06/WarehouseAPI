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

                //  var  users = await _db.Users.SingleOrDefaultAsync(e=>e.Username == user.Username && e.Password == HashPassword(user.Password!));
                 

                var users = await _db.Users
               .Where(e => e.Username == user.Username && e.Password == HashPassword(user.Password!))
               .Join(_db.Divisions, u => u.DV_ID, dv => dv.DV_ID, (u, dv) => new { User = u, Division = dv })
               .Join(_db.Positions, x => x.User.P_ID, pt => pt.P_ID, (x, pt) => new UsersDto
               {
                   ID = x.User.ID,
                   UserID = x.User.UserID,
                   Username = x.User.Username,
                   FirstName = x.User.FirstName,
                   LastName = x.User.LastName,
                   ImageFile = x.User.ImageFile,
                   Status = x.User.Status,
                   // เพิ่ม properties อื่นๆ ของ UserDto ที่คุณต้องการให้มีค่าได้ตามต้องการ
                   Division = new DivisionDto
                   {
                       DV_ID = x.Division.DV_ID,
                       DV_Name = x.Division.DV_Name
                       // เพิ่ม properties อื่นๆ ของ DivisionDto ที่คุณต้องการให้มีค่าได้ตามต้องการ
                   },
                   Position = new PositionDto
                   {
                       P_ID = pt.P_ID,
                       P_Name = pt.P_Name
                       // เพิ่ม properties อื่นๆ ของ PositionDto ที่คุณต้องการให้มีค่าได้ตามต้องการ
                   }
               })
               .SingleOrDefaultAsync();

                if (users != null)
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
