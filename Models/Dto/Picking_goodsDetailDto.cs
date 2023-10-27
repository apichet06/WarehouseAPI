using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Warehouse_API.Models.Dto
{
    public class Picking_goodsDetailDto
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(11)]
        public string? Picking_goodsID { get; set; }
        [MaxLength(10)]
        public string? ProductID { get; set; } //รหัสสินค้า
        [Range(1, 9999999)]
        public int QTYWithdrawn { get; set; } //จำนวนเบิก
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } //ราคาต่อหน่วย
        [DataType(DataType.DateTime)]
        public DateTime WithdrawnDate { get; set; } //เวลาเบิก
        [MaxLength(10)] 
        public string? WithdrawnBy { get; set; } //UserID
        [MaxLength(10)]
        public string? ApproveBy { get; set; } //ผู้อนุมัติ
        [DataType(DataType.DateTime)]
        public DateTime AppvDate { get; set; } // เวลาอนุมัติ
        [MaxLength(30)]
        public string? RequestCode { get; set; } // รหัส orderId RequestCode  ถ้าเป็นค่าว่างแสดงว่ายังไม่กดส่งเพื่อเบิก ใบเบิกยังไม่เกิดขึ้น
        [MaxLength(1)]
        public string? IsApproved { get; set; } = "i"; //status การอนุมัติ
        public UsersDto? Users  { get; set; }
        public ProductDto? Product  { get; set; }
    }
}
