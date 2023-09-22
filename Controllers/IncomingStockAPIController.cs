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
    public class IncomingStockAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly ResponseDto _resposne;
        private readonly MessageDto _message;
        private readonly IMapper _mapper;

        public IncomingStockAPIController(AppDBContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _resposne = new ResponseDto();
            _message = new MessageDto();
        }

        [HttpGet("test")]
        public IActionResult index()
        {
            try
            {
                int id = 0;
                int id2 = 1;
                var data =  id2 / id; // ข้อมูลตัวอย่าง
                return Ok(data); // ส่งข้อมูลเป็น 200 OK response

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<IncomingStock> objList = await _db.IncomingStocks.ToListAsync();
                _resposne.Result = _mapper.Map< IEnumerable<IncomingStockDto >> (objList);

            }catch (Exception ex)
            {
                _resposne.IsSuccess = false;
                _resposne.Message = _message.an_error_occurred + ex.Message;
            }

            return _resposne;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] IncomingStock incomingStock)
        {
            try
            {
                IncomingStock obj = _mapper.Map<IncomingStock>(incomingStock);
                string NextID = await GenerateAutoId();
                 
                obj.IncomingStockID = NextID; 
                _db.IncomingStocks.Add(obj);
                await _db.SaveChangesAsync();

                Product? product = await _db.Products.FirstOrDefaultAsync(c=>c.ProductID == obj.ProductID);
                 
                if (product != null)
                {
                    product.QtyInStock = product.QtyInStock + obj.QtyReceived;
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();

                }
                else
                {
                    _resposne.IsSuccess =false;
                    _resposne.Message = _message.Not_found;
                }

                 
                _resposne.Result = _mapper.Map<IncomingStockDto>(obj);
                _resposne.Message = _message.InsertMessage;
                 
            }catch (Exception ex)
            {
                _resposne.IsSuccess = false;
                _resposne.Message = _message.an_error_occurred + ex.Message;
            }
            return _resposne;
        }
 
        private async Task<string> GenerateAutoId()
        { 

                string? lastId = await _db.IncomingStocks.OrderByDescending(c=>c.ID).Select(c=>c.IncomingStockID).FirstOrDefaultAsync();

                if(!string.IsNullOrEmpty(lastId))
                {
                    int num = int.Parse(lastId.Substring(3)) + 1;
                    return "PIN" + num.ToString("D7");
                }

            return "PIN0000001";
  
        }
         

    }
     
}
