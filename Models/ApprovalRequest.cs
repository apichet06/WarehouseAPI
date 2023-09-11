using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models
{
    public class ApprovalRequest
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)]
        public int ApprovalRequestID { get; set; }
        [MaxLength(10)]
        public string? UserID { get; set; } // ผู้ใช้ที่ส่งคำขอ
        // public Guid UserID { get; set; } // ผู้ใช้ที่ส่งคำขอ
        [MaxLength(10)]
        public int ProductID { get; set; } // สินค้าที่ขออนุมัติ
        [MaxLength(10)]
        public int Quantity { get; set; } // จำนวนที่ขออนุมัติ
        [DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; } // วันที่ส่งคำขอ
        
        public bool IsApproved { get; set; } // สถานะการอนุมัติ (true หรือ false)
    }
}
