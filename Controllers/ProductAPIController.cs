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
        public async Task<ResponseDto> Post([FromBody] Product product)
        {
            try
            {
                
                 if(await _db.Products.AnyAsync(a=>a.ProductName == product.ProductName)
                {
                    _response.IsSuccess = false;
                    _response.Message = _message.already_exists + product.ProductName;
                    return _response;
                } 
                 


            }catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = _message.an_error_occurred+ex.Message;
            }
            return _response;
        }
    }
}
