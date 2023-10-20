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
    public class InventoryRequestController : ControllerBase
    {
        private AppDBContext _db;
        private ResponseDto _response;
        private MessageDto _message;
        private IMapper _mapper;

        public InventoryRequestController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();

        }


        [HttpPost("UserId")]
        public async Task<ResponseDto> CreateInventory([FromBody] InventoryRequest inventoryRequest, string UserId)
        {
            try {
                
                InventoryRequest obj =   _mapper.Map<InventoryRequest>(inventoryRequest);

                string NexId = await GenerateAutoId();

                obj.RequestCode = NexId;
                await _db.InventoryRequests.AddAsync(obj); 
                await _db.SaveChangesAsync();

                Picking_goodsDetail? objList = _db.picking_GoodsDetails.FirstOrDefault(a => a.RequestCode == "" && a.WithdrawnBy == UserId);

                // ทำการอัปเดตข้อมูล RequestCode ในตาราง picking_GoodsDetail
                if (objList != null)
                {
                    objList.RequestCode = NexId;
                    _db.picking_GoodsDetails.Update(objList);
                    await _db.SaveChangesAsync();
                }

                _response.Result = _mapper.Map<InventoryRequestDto>(obj);
                _response.Message = _message.InsertMessage;
            
            }catch (Exception ex)
            {
                 _response.Message = _message.an_error_occurred + ex.Message;
            }

           return  _response;
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
