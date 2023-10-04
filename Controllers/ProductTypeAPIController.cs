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
    public class ProductTypeAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly MessageDto _message;

        public ProductTypeAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _message =  new MessageDto();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Index()
        {
            try
            {
                IEnumerable<ProductType> productType = await _db.ProductTypes.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductTypeDto>>(productType);
           


            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateProductType([FromBody] ProductType productType)
        {
            try
            {
                if(await _db.ProductTypes.AnyAsync(a=>a.TypeName == productType.TypeName))
                {
                    _response.IsSuccess=false;
                    _response.Message = _message.already_exists + productType.TypeName;
                    return _response;
                }

                string NextID = await GenerateAutoID();

                ProductType obj = _mapper.Map<ProductType>(productType);
                obj.TypeID = NextID;
                _db.ProductTypes.Add(obj);
                await _db.SaveChangesAsync(); 


                _response.Result = _mapper.Map<ProductTypeDto>(obj);
                _response.Message = _message.InsertMessage;

            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> UpdateProductType([FromBody] ProductType productType ,int id)
        {
            try
            {
                if(await _db.ProductTypes.AnyAsync(c=>c.ID != id && c.TypeName == productType.TypeName))
                {
                    _response.IsSuccess=false;
                    _response.Message = _message.already_exists+productType.TypeName;
                    return _response;
                }

                ProductType? obj = await _db.ProductTypes.FirstOrDefaultAsync(c => c.ID == id);
                

                obj!.TypeName = productType.TypeName;
                _db.ProductTypes.Update(obj);
                await _db.SaveChangesAsync();

                 _response.Result = _mapper.Map<ProductType>(productType);
                 _response.Message = _message.UpdateMessage;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred+ex.Message;
            }

            return _response;
        }


        [HttpDelete("{typeID}")] 
        public async Task<ActionResult<ResponseDto>> DeleteProductType(string typeID)
        {
            try
            {

                if (await _db.Products.AnyAsync(c=> c.TypeID == typeID))
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.Active + typeID;
                }
                else
                {
                    ProductType? obj = await _db.ProductTypes.FirstOrDefaultAsync(c => c.TypeID == typeID);
                    if (obj != null)
                    {
                        _db.ProductTypes.Remove(obj);
                         await _db.SaveChangesAsync();

                        _response.IsSuccess = true;
                        _response.Message = _message.DeleteMessage;
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = _message.Not_found;
                    }
                }
            

            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = _message.an_error_occurred + ex.Message;
            }
            return _response;
        }


        private async Task<string> GenerateAutoID()
        {
            string? LastId = await _db.ProductTypes.OrderByDescending(c=>c.ID).Select(c=> c.TypeID).FirstOrDefaultAsync();   
            if(LastId != null)
            {
                int num = int.Parse(LastId.Substring(3))+1;
                return "TPY"+ num.ToString("D3");
            }
            return "TPY001";
        }

    }
}
