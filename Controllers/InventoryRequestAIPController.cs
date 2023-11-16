using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;
using Warehouse_API.Data;
using Warehouse_API.Models;
using Warehouse_API.Models.Dto;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryRequestAIPController : ControllerBase
    {
        private AppDBContext _db;
        private ResponseDto _response;
        private MessageDto _message;
        private IMapper _mapper;

        public InventoryRequestAIPController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();

        }

      

          [HttpGet]
          public async Task<ResponseDto>Get() {
              try
              {

                var querys = from a in _db.InventoryRequests
                             join b in _db.Users on a.RequesterUserId equals b.UserID
                             join d in _db.Divisions on a.DivisionId equals d.DV_ID
                             join c in _db.Users on a.ApproveBy equals c.UserID into leftJoinC
                             from cData in leftJoinC.DefaultIfEmpty()
                             join e in _db.Divisions on cData.DV_ID equals e.DV_ID into leftJoinE
                             from eData in leftJoinE.DefaultIfEmpty()
                             // where a.ApproveBy == null
                             select new InventoryRequestDto
                             {
                                 ID = a.ID,
                                 RequestCode = a.RequestCode,
                                 TransactionTime = a.TransactionTime,
                                 RequesterUserId = a.RequesterUserId,
                                 DivisionId = a.DivisionId,
                                 ApproveBy = a.ApproveBy,
                                 AppvDate = a.AppvDate,
                                 Purpose = a.Purpose,
                                 IsApproved = a.IsApproved,
                                 Users = new UsersDto
                                 {
                                     FirstName = b.FirstName,
                                     LastName = b.LastName,
                                     Status = b.Status,
                                     Division = new DivisionDto
                                     {
                                         DV_ID = d.DV_ID,
                                         DV_Name = d.DV_Name,
                                     }
                                 },
                                 ApprovedUsers = new UsersDto
                                 {
                                     FirstName = cData != null ? cData.FirstName : null,
                                     LastName = cData != null ? cData.LastName : null,
                                     Division = new DivisionDto
                                     {
                                         DV_ID = eData.DV_ID,
                                         DV_Name =  eData.DV_Name 
                                     }
                                 }
                             }; 
                   
                  List<InventoryRequestDto> objlist = await querys.ToListAsync();
                  _response.Result = objlist;
              }
              catch (Exception ex)
              {

                  _response.IsSuccess = false;
                  _response.Message = _message.an_error_occurred + ex.Message;
              }
              return  _response;
          } 


        [HttpGet("Picking_GoodsDetail/{RequestCode}")]
        public async Task<ResponseDto> GetPicking(string RequestCode)
        {
            try
            {
                var query = from pg in _db.Picking_GoodsDetails
                            join p in _db.Products on pg.ProductID equals p.ProductID
                            join t in _db.ProductTypes on p.TypeID equals t.TypeID
                            join u in _db.Users on pg.WithdrawnBy equals u.UserID
                            where pg.RequestCode == RequestCode
                            select new Picking_goodsDetailDto
                            {
                                ID = pg.ID,
                                Picking_goodsID = pg.Picking_goodsID,
                                ProductID = pg.ProductID,
                                QTYWithdrawn = pg.QTYWithdrawn,
                                UnitPrice = pg.UnitPrice,
                                WithdrawnDate = pg.WithdrawnDate,
                                WithdrawnBy = pg.WithdrawnBy,
                                ApproveBy = pg.ApproveBy,
                                AppvDate = pg.AppvDate,
                                RequestCode = pg.RequestCode,
                                IsApproved = pg.IsApproved,
                                Users = new UsersDto { 
                                  UserID = u.UserID,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName, 
                                },
                                Product = new ProductDto
                                {
                                    ProductID = p.ProductID,
                                    ProductName = p.ProductName,
                                    ProductType = new ProductTypeDto()
                                    {
                                        TypeName = t.TypeName
                                    }
                                }
                                 
                            };

                List<Picking_goodsDetailDto> objList = await query.ToListAsync();
                _response.Result = objList;

            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message; 
            }
            return _response;
        }





        //สร้าง Order การเบิก

        [HttpPost("{UserId}")]
        public async Task<ResponseDto> CreateInventory([FromBody] InventoryRequest aentoryRequest, string UserId)
        {
            try
            {
                InventoryRequest obj = _mapper.Map<InventoryRequest>(aentoryRequest);
                string NexId = await GenerateAutoId();
                obj.RequestCode = NexId;
                obj.TransactionTime = DateTime.Now;
                obj.AppvDate = null;
                obj.RequesterUserId = UserId;

                await _db.InventoryRequests.AddAsync(obj);
                await _db.SaveChangesAsync();

                var itemsToUpdate = await _db.Picking_GoodsDetails.Where(a => a.RequestCode == null && a.WithdrawnBy == UserId).ToListAsync();

                if (itemsToUpdate != null && itemsToUpdate.Count > 0) // มีข้อมูลหรือไม่ 
                {
                    foreach (var item in itemsToUpdate) // forloop
                    {
                        item.RequestCode = NexId;
                        _db.Picking_GoodsDetails.Update(item); 
                    }

                    await _db.SaveChangesAsync();
                }

                _response.Result = _mapper.Map<InventoryRequestDto>(obj);
                _response.Message = _message.InsertMessage;
            }
            catch (Exception ex)
            {
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }






        //Update ข้อมูลการเบิกของ

        [HttpPut("{UserId}")]
        public async Task<ResponseDto> UpdateInventory([FromBody] InventoryRequest aentoryRequest, string UserId)
        {
            try
            { 
                var obj = await _db.InventoryRequests.FirstOrDefaultAsync(a => a.RequestCode == aentoryRequest.RequestCode);
                obj!.ApproveBy = UserId;
                obj.AppvDate = DateTime.Now;
                obj.IsApproved = aentoryRequest.IsApproved;  
                _db.InventoryRequests.Update(obj);
                await _db.SaveChangesAsync();

                 var itemsToUpdate = await _db.Picking_GoodsDetails.Where(a => a.RequestCode == aentoryRequest.RequestCode && a.WithdrawnBy == UserId).ToListAsync();
               
                if (itemsToUpdate != null && itemsToUpdate.Count > 0) // มีข้อมูลหรือไม่ 
                {
                    foreach (var item in itemsToUpdate) // forloop
                    {
                        item.ApproveBy = UserId;
                        item.AppvDate = DateTime.Now;
                        item.IsApproved = aentoryRequest.IsApproved!;
                        _db.Picking_GoodsDetails.Update(item);
                        if(aentoryRequest.IsApproved == "Y")
                        {
                            var product = await _db.Products.FirstOrDefaultAsync(c => c.ProductID == item.ProductID); 
                            if (product != null)
                                {
                                    product.QtyInStock -= item.QTYWithdrawn;
                                    _db.Products.Update(product);
                                }

                        }
                       
                    }

                    await _db.SaveChangesAsync();
                } 

                _response.Result = _mapper.Map<InventoryRequestDto>(obj);
                _response.Message = _message.InsertMessage;
            }
            catch (Exception ex)
            {
                _response.Message = _message.an_error_occurred + ex.Message;
            }

            return _response;
        }
         
        private async Task<string> GenerateAutoId()
        {
            string? LastId = await _db.InventoryRequests.OrderByDescending(e => e.ID).Select(e => e.RequestCode).FirstOrDefaultAsync();

            string currentYearMonth = DateTime.Now.ToString("yyyyMM");

            if (!string.IsNullOrEmpty(LastId) && LastId.StartsWith("ODIV" + currentYearMonth))
            {
                int num = int.Parse(LastId.Substring(11)) + 1;
                return "ODIV" + currentYearMonth + "-" + num.ToString("D7");
            }

            return "ODIV" + currentYearMonth + "-0000001";
        }

    }
}
