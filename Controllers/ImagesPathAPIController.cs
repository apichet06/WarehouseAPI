using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesPathAPIController : ControllerBase
    {
        [HttpGet("{*imagePath}")]
        public async Task<IActionResult> GetImage(string imagePath)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

                // if (System.IO.File.Exists(filePath))
                // {
                string ext = Path.GetExtension(filePath).ToLower();
                string contentType = ext switch
                {
                    ".png" => "image/png",
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    _ => "application/octet-stream" // สามารถเปลี่ยนเป็น "image/jpeg" หรือ "image/png" ได้ตามต้องการ
                };

                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(imageBytes, contentType);
                // }

                //return NotFound();
            }
            catch (Exception ex)
            {

                // ทำการจัดการข้อผิดพลาดที่เกิดขึ้นในกรณีที่เกิดข้อผิดพลาดในระหว่างการประมวลผล
                // คุณสามารถทำการบันทึกหรือจัดการข้อผิดพลาดอื่น ๆ ตามความเหมาะสม
                return StatusCode(500, ex.Message);
            }
        }

    }
}
