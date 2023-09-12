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
    public class ApprovalRequestAPIController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly AppDBContext _db;
        private readonly MessageDto _message;
        private readonly IMapper _mapper;
        public ApprovalRequestAPIController(AppDBContext db, IMapper mapper) {
            
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message = new MessageDto();
        
        }

        [HttpPost] 
      public async Task<ResponseDto> Post([FromBody] ApprovalRequest approval)
        {
            try
            {
                string NextID = await GenerateAutoID();
                

                ApprovalRequest? obj = _mapper.Map<ApprovalRequest>(approval);
                obj.ApprovalRequestID = NextID;
                _db.Approvals.Add(obj);
               await _db.SaveChangesAsync();
              
               _response.Result = _mapper.Map<ApprovalRequestDto>(approval);
                _response.Message = _message.InsertMessage;
              
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred +ex.Message;
            }



            return _response;
        }


    
        private async Task<string> GenerateAutoID()
        {
            string? LastId = await _db.Approvals.OrderByDescending(u => u.ID).Select(x => x.ApprovalRequestID).FirstOrDefaultAsync(); 
            if (LastId == null && LastId!.StartsWith("AP"))
            {
              
                int num = int.Parse(LastId.Substring(1)) + 1;
                return "AP" + num.ToString("D7");
            }
            return "AP0000001";
        }
      
    }
}
