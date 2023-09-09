using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models.Dto
{
    public class UsersDto
    {
        [Key]
        public int UserID { get; set; }
        [MaxLength(100)]
        public string? Username { get; set; }
        [MaxLength(100)]
        public string? Password { get; set; } = "123456";
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(10)]
        public string? DV_ID { get; set; }
        [MaxLength(10)]
        public string? P_ID { get; set;}
        [MaxLength(50)]
        public string? Status { get;set; }
        // เพิ่มคอลัมน์เพิ่มเติมตามความต้องการของระบบผู้ใช้งาน
    }
}
