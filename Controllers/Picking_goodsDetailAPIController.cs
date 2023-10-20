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
    public class Picking_goodsDetailAPIController : ControllerBase
    {
        private AppDBContext _db;
        private ResponseDto _response;
        private MessageDto _message;
        private IMapper _mapper;

        public Picking_goodsDetailAPIController(AppDBContext db,IMapper mapper )
        {
            _db = db;
            _response =  new ResponseDto();
            _message = new MessageDto();
            _mapper = mapper;
         
        }
        [HttpGet("UserId")]
        public async Task<ResponseDto> Get(string UserId)
        {
            try
            {
                List<Picking_goodsDetail> objList = await _db.picking_GoodsDetails.Where(e => e.WithdrawnBy == UserId).OrderByDescending(e => e.ProductID).ToListAsync();

                var Data = from pg in _db.picking_GoodsDetails
                           join u in _db.Products on pg.ProductID equals u.ProductID
                           select new Picking_goodsDetailDto
                           {
                               ProductID = pg.ProductID,
                               QTYWithdrawn = pg.QTYWithdrawn, 
                           };


                _response.Result = _mapper.Map<IEnumerable<Picking_goodsDetailDto>>(objList);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] Picking_goodsDetail outgoingStock)
        {
            try
            {
               Picking_goodsDetail obj = _mapper.Map <Picking_goodsDetail>(outgoingStock);

                string NextId = await GenerateAutoId();

                obj.Picking_goodsID = NextId; 
                _db.picking_GoodsDetails.Add(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<Picking_goodsDetailDto>(obj);
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
        public async Task<ResponseDto> Put(int id, [FromBody]Picking_goodsDetail outgoingStock)
        {
            try
            {
               Picking_goodsDetail? obj = await _db.picking_GoodsDetails.FirstOrDefaultAsync(c => c.ID == id);
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
                obj.AppvDate = DateTime.Now;

                _db.picking_GoodsDetails.Update(obj);
                await _db.SaveChangesAsync();


                product!.QtyInStock -= obj.QTYWithdrawn;

                _db.Products.Update(product);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<Picking_goodsDetailDto>(obj);
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
            string? lastID  = await _db.picking_GoodsDetails.OrderByDescending(x => x.ID).Select(x=>x.Picking_goodsID).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(lastID))
            {
                int num = int.Parse(lastID.Substring(4))+1;
                return "POUT" + num.ToString("D7");
            }

            return "POUT0000001";
        } 
 
    }
}
