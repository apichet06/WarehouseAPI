using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
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

        [HttpGet("Chart")]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var Data = from a in _db.Picking_GoodsDetails
                           join b in _db.Products on a.ProductID equals b.ProductID
                           where a.IsApproved == "Y"
                           group new { a, b } by a.ProductID into g
                           select new Picking_goodsDetailDto
                           {   
                               ID = g.Max(a=>a.a.ID),
                               QTYWithdrawn = g.Sum(x => x.a.QTYWithdrawn),
                               IsApproved = g.Max(x => x.a.IsApproved),
                               Product = new ProductDto
                               {
                                   ProductName = g.Max(x => x.b.ProductName)
                               }
                           };

                List<Picking_goodsDetailDto> objList = await Data.ToListAsync();

                _response.Result = objList;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }


        [HttpGet("Pickinglist/{UserId}")]
        public async Task<ResponseDto> Get(string UserId)
        {
            try
            {
               // List<Picking_goodsDetail> objList = await _db.Picking_GoodsDetails.Where(e => e.WithdrawnBy == UserId).OrderByDescending(e => e.ProductID).ToListAsync();

                var Data = from pg in _db.Picking_GoodsDetails
                           join p in _db.Products on pg.ProductID equals p.ProductID
                           join u in _db.Users on pg.WithdrawnBy equals  u.UserID
                           where u.UserID == UserId && pg.RequestCode ==null
                           select new Picking_goodsDetailDto
                           {
                               ID = pg.ID,
                               Picking_goodsID = pg.Picking_goodsID,
                               ProductID = pg.ProductID,
                               QTYWithdrawn = pg.QTYWithdrawn,
                               UnitPrice = pg.UnitPrice, 
                               Users = new UsersDto { 
                                   UserID = u.UserID,
                                   FirstName = u.FirstName,
                                   LastName= u.LastName, 
                               },
                               Product = new ProductDto
                               {
                                   ProductID = p.ProductID,
                                   ProductName= p.ProductName,
                                   UnitOfMeasure= p.UnitOfMeasure,
                               }
                                
                           };

                List<Picking_goodsDetailDto> objList = await Data.ToListAsync();

                _response.Result = objList;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<ResponseDto> Post([FromBody] Picking_goodsDetail outgoingStock)
        {
            try
            {
                Picking_goodsDetail? product = await _db.Picking_GoodsDetails.FirstOrDefaultAsync(x =>
                    x.ProductID == outgoingStock.ProductID && x.RequestCode == null && x.WithdrawnBy == outgoingStock.WithdrawnBy);

                Picking_goodsDetail obj = _mapper.Map<Picking_goodsDetail>(outgoingStock);

                if (product == null)
                {
                    // ไม่มีข้อมูล ดำเนินการ Insert
                    string NextId = await GenerateAutoId();
                    obj.WithdrawnDate = DateTime.Now;
                    obj.AppvDate = null;
                    obj.Picking_goodsID = NextId;
                    _db.Picking_GoodsDetails.Add(obj);
                }
                else
                {
                    // มีข้อมูลอยู่แล้ว ดำเนินการ Update
                    product!.WithdrawnDate = DateTime.Now;
                    product.QTYWithdrawn = product.QTYWithdrawn += obj.QTYWithdrawn;
                    _db.Picking_GoodsDetails.Update(product);
                }

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

        [HttpPut("Qty/{id:int}")]
        
        public async Task<ResponseDto> UpdateQty([FromBody] Picking_goodsDetail pGoodsDetail ,int id)
        {
            try
            {
               Picking_goodsDetail? obj = await _db.Picking_GoodsDetails.FirstOrDefaultAsync(a=>a.ID==id);
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                  return _response;
                }

                obj.QTYWithdrawn = pGoodsDetail.QTYWithdrawn;
                _db.Picking_GoodsDetails.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<Picking_goodsDetailDto>(obj);
                _response.Message= _message.UpdateMessage;


            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred+ ex.Message;
            }
            return _response;
        }
  

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ResponseDto> Put(int id, [FromBody]Picking_goodsDetail outgoingStock)
        {
            try
            {
               Picking_goodsDetail? obj = await _db.Picking_GoodsDetails.FirstOrDefaultAsync(c => c.ID == id);
                Product? product = await _db.Products.FirstOrDefaultAsync(c=>c.ProductID == obj!.ProductID);
                bool productExists = _db.Products.Any(p => p.ProductID == outgoingStock.ProductID);
                if (!productExists)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Not_found;
                    return _response;
                }
                
                obj!.IsApproved = outgoingStock.IsApproved;
                obj.QTYWithdrawn = outgoingStock.QTYWithdrawn;
                obj.ApproveBy = outgoingStock.ApproveBy;
                obj.AppvDate = DateTime.Now;

                _db.Picking_GoodsDetails.Update(obj);
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
         
        [HttpDelete("{id:int}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Picking_goodsDetail? obj = await _db.Picking_GoodsDetails.FirstOrDefaultAsync(e => e.ID == id);

                if (obj == null)
                {
                    _response.IsSuccess =false;
                    _response.Message = _message.Not_found;
                 return _response;
                }

                _db.Picking_GoodsDetails.Remove(obj);
                await _db.SaveChangesAsync();

                
                _response.Result =   _mapper.Map<Picking_goodsDetailDto>(_db);
                _response.Message = _message.DeleteMessage;


            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred  + ex.Message;
            }


            return _response;
        }

         
        private async Task<string> GenerateAutoId()
        {
            string? lastID  = await _db.Picking_GoodsDetails.OrderByDescending(x => x.ID).Select(x=>x.Picking_goodsID).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(lastID))
            {
                int num = int.Parse(lastID.Substring(4))+1;
                return "POUT" + num.ToString("D7");
            }

            return "POUT0000001";
        } 
 
    }
}
