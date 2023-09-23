using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse_API.Data;
using Warehouse_API.Models;
using Warehouse_API.Models.Dto;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {

        private AppDBContext _db;
        private ResponseDto _response;
        private MessageDto _message;
        private IMapper _mapper;


        public ProductAPIController(AppDBContext db,  IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();
        }  
         
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Product> objList = await _db.Products.ToListAsync();
               _response.Result =  _mapper.Map<IEnumerable<ProductDto>>(objList);
                

            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message =  _message.an_error_occurred + ex.Message;
                return StatusCode(500, _response);
            }
            return Ok(_response);
        }
         

        [HttpPost]
        public async Task<ResponseDto> Post([FromForm] Product product,IFormFile imageFile = null!)
        {
            try
            {
                
                 if(await _db.Products.AnyAsync(a=>a.ProductName == product.ProductName))
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.already_exists + product.ProductName;
                    return _response;
                }
                 string NextId =await GenerateAutoID();
            

                if (imageFile != null && imageFile.Length > 0 )
                {
                    string uploadsFolderPath = Path.Combine("Uploads\\Product", DateTime.Now.ToString("yyyyMM"));
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }
               
                 string extension = Path.GetExtension(imageFile.FileName);
                 string uniqueFileName = NextId + extension;
                 string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    product.PImages = filePath;
                }
                DateTime bangkokTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok"));
                Product obj = _mapper.Map<Product>(product);
                 obj.ProductID = NextId;
                 obj.ReceiveAt = bangkokTime;
                _db.Products.Add(obj);
                await _db.SaveChangesAsync();   
                 
                _response.Result = _mapper.Map<ProductDto>(obj);
                _response.Message = _message.InsertMessage;
                  
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = _message.an_error_occurred+ex.Message;
            }
            return _response;
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<ResponseDto> Put([FromForm] Product product, int id, IFormFile imageFile = null!)
        {
            try
            {
                Product obj  = await _db.Products.FirstAsync(x => x.ID == id);
                 if(obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }

                if (await _db.Products.AnyAsync(a=>a.ID != id && a.ProductName  == product.ProductName))
                {

                    _response.IsSuccess = false;
                    _response.Message = _message.already_exists + product.ProductName;

                    return _response;
                     
                }
                 
                if(imageFile != null && imageFile.Length >0 )
                {
                    if (!string.IsNullOrEmpty(obj.PImages))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), obj.PImages);
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    string uploadsFolderPath = Path.Combine("Uploads\\Product", DateTime.Now.ToString("yyyyMM"));
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = obj.ProductID + extension;
                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);


                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    obj.PImages = filePath;

                }


                obj.ProductName = product.ProductName;
                obj.ProductDescription = product.ProductDescription;
                obj.UnitPrice = product.UnitPrice;
                obj.UnitOfMeasure = product.UnitOfMeasure;
                obj.QtyInStock = product.QtyInStock;
                obj.QtyMinimumStock = product.QtyMinimumStock;
                obj.TypeID = product.TypeID;
                // รับวันที่และเวลาปัจจุบันในโซนเวลาของกรุงเทพมหานคร
                DateTime bangkokTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok"));
               
                obj.lastAt = bangkokTime;


                _db.Products.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<Product>(obj);
                _response.Message = _message.UpdateMessage;

                 

            }catch (Exception ex)
            {
                _response.IsSuccess=false;
                //แจ้งเตื่อน Server error
                _response.Message = _message.an_error_occurred + ex.Message;
            }

              

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                 Product? obj = await _db.Products.FirstOrDefaultAsync(c=> c.ID == id);

                if (obj == null)
                {
                    _response.IsSuccess=false;
                    _response.Message = _message.Not_found;
                    return _response;
                }
                if (string.IsNullOrEmpty(obj.PImages))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), obj.PImages!);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _db.Products.Remove(obj);
                await _db.SaveChangesAsync();

                _response.Result= _mapper.Map<ProductDto>(obj);
                _response.Message = _message.DeleteMessage;
               
            } catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = _message.an_error_occurred + ex.Message + Environment.NewLine;
            }
            return _response;
        }

         
        private async Task<string> GenerateAutoID()
        {
            string? LastId = await _db.Products.OrderByDescending(u => u.ID).Select(x => x.ProductID).FirstOrDefaultAsync();
            if (LastId != null && LastId.StartsWith("PRO"))
            {
                int num = int.Parse(LastId.Substring(3)) + 1;
                return "PRO" + num.ToString("D7");

            }
            return "PRO0000001";
        }
       

    }
}
