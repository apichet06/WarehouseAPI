using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models
{
    public class OutgoingStock
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(11)]
        public string? OutgoingStockID { get; set; }
        [MaxLength (10)]
        public string? ProductID { get; set; }
         [Range (1, 9999999)] 
        public int QTYWithdrawn { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } //ราคาต่อหน่วย
        [DataType(DataType.DateTime)]
        public DateTime WithdrawnDate { get; set; }
        [MaxLength(10)]
        public string? WithdrawnBy { get; set; } //UserID
        [MaxLength(10)]
        public string? ApproveBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime AppvDate { get; set; } // เวลาอนุมัติ
        [MaxLength(1)]
        public string IsApproved { get; set; } = "";

    }
}
