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
    public class OutgoingStockAPIController : ControllerBase
    {
        private AppDBContext _db;
        private ResponseDto _response;
        private MessageDto _message;
        private IMapper _mapper;

        public OutgoingStockAPIController(AppDBContext db,IMapper mapper )
        {
            _db = db;
            _response =  new ResponseDto();
            _message = new MessageDto();
            _mapper = mapper;
         
        }
        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<OutgoingStock> objList = await _db.OutgoingStocks.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<OutgoingStockDto>>(objList);

            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] OutgoingStock outgoingStock)
        {
            try
            {
                 OutgoingStock obj = _mapper.Map<OutgoingStock>(outgoingStock);

                string NextId = await GenerateAutoId();

                obj.OutgoingStockID = NextId; 
                _db.OutgoingStocks.Add(obj);
                await _db.SaveChangesAsync();
                             
                _response.Result = _mapper.Map<OutgoingStockDto>(obj);
                _response.Message = _message.InsertMessage;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ResponseDto> Put(int id, [FromBody] OutgoingStock outgoingStock)
        {
            try
            {
                OutgoingStock? obj = await _db.OutgoingStocks.FirstOrDefaultAsync(c => c.ID == id);
                Product? product = await _db.Products.FirstOrDefaultAsync(c=>c.ProductID == obj!.ProductID);
                bool productExists = _db.Products.Any(p => p.ProductID == outgoingStock.ProductID);
                if (!productExists)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                }
                
                obj!.IsApproved = outgoingStock.IsApproved;
                obj.QTYWithdrawn = outgoingStock.QTYWithdrawn;
                obj.ApproveBy = outgoingStock.ApproveBy;
                obj.AppvDate = outgoingStock.AppvDate;

                _db.OutgoingStocks.Update(obj);
                await _db.SaveChangesAsync();


                product!.QtyInStock -= obj.QTYWithdrawn;

                _db.Products.Update(product);
                await _db.SaveChangesAsync();
 
                _response.Result = _mapper.Map<OutgoingStockDto>(obj);
                _response.Message = _message.Approved_status;

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
            string? lastID  = await _db.OutgoingStocks.OrderByDescending(x => x.ID).Select(x=>x.OutgoingStockID).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(lastID))
            {
                int num = int.Parse(lastID.Substring(4))+1;
                return "POUT" + num.ToString("D7");
            }

            return "POUT0000001";
        } 
 
    }
}
