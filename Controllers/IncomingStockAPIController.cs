using AutoMapper;
using Azure;
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
        private readonly ResponseDto _response;
        private readonly MessageDto _message;
        private readonly IMapper _mapper;

        public IncomingStockAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();
        }

    /* [HttpGet("test")]
        public IActionResult index()
        {
            try
            {
                int id = 0;
                int id2 = 1;
                var data = id2 / id; // ข้อมูลตัวอย่าง
                return Ok(data); // ส่งข้อมูลเป็น 200 OK response

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    */

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<IncomingStock> objList = await _db.IncomingStocks.ToListAsync();

                var Data = from incoming in _db.IncomingStocks
                           join u in _db.Users on incoming.ReceivedBy equals u.UserID
                           join p in _db.Products on incoming.ProductID equals p.ProductID
                           select new IncomingStockDto
                           {
                               ID = incoming.ID,
                               IncomingStockID = incoming.IncomingStockID,
                               ProductID = incoming.ProductID,
                               QtyReceived = incoming.QtyReceived,
                               UnitPriceReceived = incoming.UnitPriceReceived,
                               ReceivedDate = incoming.ReceivedDate,
                               ReceivedBy = incoming.ReceivedBy,

                               Users = new UsersDto
                               {
                                   FirstName = u.FirstName,
                                   LastName = u.LastName
                               },
                               Product = new ProductDto
                               {
                                   ProductID = p.ProductID,
                                   ProductName = p.ProductName,
                                   UnitOfMeasure = p.UnitOfMeasure,
                               }

                           };
                 

                List<IncomingStockDto> objlist = await  Data.ToListAsync();
                _response.Result = objlist;
                 



            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }


        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] IncomingStock incomingStock)
        {
            try
            {
                IncomingStock obj = _mapper.Map<IncomingStock>(incomingStock);
                string NextID = await GenerateAutoId();

                obj.IncomingStockID = NextID;
                obj.ReceivedDate = DateTime.Now;
                _db.IncomingStocks.Add(obj);
                await _db.SaveChangesAsync();

                Product? product = await _db.Products.FirstOrDefaultAsync(c => c.ProductID == obj.ProductID);

                if (product != null)
                {
                    product.QtyInStock = product.QtyInStock + obj.QtyReceived;
                    product.UnitPrice = obj.UnitPriceReceived;
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();

                }

                else
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                }

                _response.Result = _mapper.Map<IncomingStockDto>(obj);
                _response.Message = _message.InsertMessage;

            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")] 
        public async Task<ResponseDto> DeleteIncom(int id)
        {
            try
            { 
                IncomingStock? obj = await _db.IncomingStocks.FirstOrDefaultAsync(c => c.ID == id);

                if (obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }

               Product? product = await _db.Products.FirstOrDefaultAsync(c => c.ProductID == obj!.ProductID);

                if (product != null)
                {
                    product.QtyInStock = product.QtyInStock -= obj!.QtyReceived;
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }
                 

                _db.IncomingStocks.Remove(obj!);
                await _db.SaveChangesAsync();


                _response.Message = _message.DeleteMessage;
                _response.Result = _mapper.Map<IncomingStock>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = ex.Message;
            }

            return _response;


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
