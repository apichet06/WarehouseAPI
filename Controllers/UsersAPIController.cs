using AutoMapper;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Warehouse_API.Data;
using Warehouse_API.Models;
using Warehouse_API.Models.Dto;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;
        private MessageDto _message;

        public UsersAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var query = from user in _db.Users
                            join dv in _db.Divisions on user.DV_ID equals dv.DV_ID
                            join pt in _db.Positions on user.P_ID equals pt.P_ID
                            select new UsersDto
                            {
                                ID = user.ID,
                                UserID = user.UserID,
                                Username = user.Username,
                                Password = user.Password,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                ImageFile = user.ImageFile,
                                DV_ID = user.DV_ID,
                                P_ID = user.P_ID,
                                Status = user.Status,
                                Date = user.Date,
                                Division = new DivisionDto
                                {
                                    DV_ID = dv.DV_ID,
                                    DV_Name= dv.DV_Name
                                    // เพิ่ม properties อื่นๆ ของ DivisionDto ที่คุณต้องการให้มีค่าได้ตามต้องการ
                                },
                                Position = new PositionDto
                                {
                                    P_ID = pt.P_ID,
                                    P_Name = pt.P_Name
                                    // เพิ่ม properties อื่นๆ ของ PositionDto ที่คุณต้องการให้มีค่าได้ตามต้องการ
                                }
                            };

                List<UsersDto> resultList = await query.ToListAsync();
                _response.Result = resultList;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromForm] Users users, IFormFile imageFile = null!)
        {
            try
            {
                if(await _db.Users.AnyAsync(c => c.Username == users.Username && c.LastName ==users.LastName && c.FirstName == users.FirstName))
                    {
                        _response.IsSuccess = false;
                        _response.Message = _message.already_exists + users.Username;
                        return _response;
                    }

                string nextID = await GenerateAutoId();
                

                if(imageFile !=null && imageFile.Length > 0)
                {
                    string uploadFolderPath = Path.Combine("Uploads\\Profile", DateTime.Now.ToString("yyyyMM"));
                    if(!Directory.Exists(uploadFolderPath))
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = nextID + extension;
                    string filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                      await  imageFile.CopyToAsync(fileStream);
                    }
                    users.ImageFile = filePath;
                }
                 
                Users obj = _mapper.Map<Users>(users);


                using (SHA256 sha256 = SHA256.Create())
                {

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(users.Password!);
                    byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                    string hashedPassword = Convert.ToBase64String(hashedBytes); 
                    obj.Password = hashedPassword;

                }
                 

                obj.UserID = nextID;
                obj.Date = DateTime.Now;
                _db.Users.Add(obj);
                await _db.SaveChangesAsync();

                 
                _response.Result = _mapper.Map<UsersDto>(obj);
                _response.Message = _message.InsertMessage;

            }catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message= _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ResponseDto> Put(int id, [FromForm] Users users,IFormFile imageFile = null!)
        {
            try
            {
                Users? obj = await _db.Users.FirstOrDefaultAsync(c=> c.ID == id);
                if(obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }
            if(await _db.Users.AnyAsync(c => c.ID != id && c.FirstName == users.FirstName && c.LastName == users.LastName && c.Username == users.Username))
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.already_exists + users.FirstName + " " + users.LastName;
                    return _response;
                }
              

                 obj.FirstName = users.FirstName;
                 obj.LastName = users.LastName;
                 obj.DV_ID = users.DV_ID;
                 obj.Status = users.Status;
                 obj.P_ID = users.P_ID;
                 obj.Status = users.Status;
                 obj.Date = DateTime.Now;
                  

                if (imageFile != null && imageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(obj.ImageFile))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(),obj.ImageFile);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    string uploadFolderPath = Path.Combine("Uploads\\Profile", DateTime.Now.ToString("yyyyMM"));
                    string extension = Path.GetExtension(imageFile!.FileName);
                    string uniqueFileName = obj.UserID + extension;
                    string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                    using (var filestream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(filestream);
                    }

                    obj.ImageFile = filePath;
                }

                 
                 
                _db.Users.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<UsersDto>(obj);
                _response.Message = _message.UpdateMessage;

            }catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message=_message.an_error_occurred + ex.Message;
            }
            return _response;
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Users?  obj =  await _db.Users.FirstOrDefaultAsync(x=>x.ID == id);
                if (obj == null)
                {
                    _response.IsSuccess=false;
                    _response.Message = _message.Not_found;
                    return _response;
                }

                if (!string.IsNullOrEmpty(obj.ImageFile))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(),obj.ImageFile!);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                 

                 _db.Users.Remove(obj);
                 await  _db.SaveChangesAsync();

                _response.Result = _mapper.Map<UsersDto>(obj);
                _response.Message = _message.DeleteMessage;
                 

            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }
         

        [HttpPut("ChangePassword/{id:int}")]
        public async Task<ResponseDto> ChangPassword(int id, Users users)
        {
            try
            {
                Users? obj = await _db.Users.FirstOrDefaultAsync(x => x.ID == id);
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }


                using (SHA256 sha256 = SHA256.Create())
                {

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(users.Password!);
                    byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                    string hashedPassword = Convert.ToBase64String(hashedBytes);

                    obj.Password = hashedPassword;

                }

                await _db.SaveChangesAsync();
                _response.Message = _message.changPassword;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }



        private async Task<string> GenerateAutoId()
        {
            string? LastId = await _db.Users.OrderByDescending(u => u.ID).Select(x=>x.UserID).FirstOrDefaultAsync();
            if(LastId != null && LastId.StartsWith("U"))
            {
                int num = int.Parse(LastId.Substring(1))+1;
                return "U"+num.ToString("D4");

            }
            return "U0001";
        }

    }
}
