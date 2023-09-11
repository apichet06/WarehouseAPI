using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models.Dto
{
    public class UsersDto
    { 

        [Key]
        public int ID { get; set; }
        [MaxLength(10)]
        public string? UserID { get; set; }
        [MaxLength(100)]
        public string? Username { get; set; }
        [MaxLength(100)]
        public string? Password { get; set; } = "123456";
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(200)]
        public string? ImageFile { get; set; }
        [MaxLength(10)]
        public string? DV_ID { get; set; } // รหัสแผนก
        [MaxLength(10)]
        public string? P_ID { get; set; } // รหัสตำแหน่ง
        [MaxLength(50)]
        public string? Status { get; set; } = "พนักงาน"; // เป็นพนักงาน /ลาออก

    }
}
