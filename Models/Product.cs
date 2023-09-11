using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models
{
    //รายการสินค้าที่มีในคลัง
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [MaxLength (10)]
        public string? ProductID { get; set; } //รหัสสินค้าอุปกรณ์
        [MaxLength(200)]
        public string? ProductName { get; set; } //สินค้าอุปกรณ์
        [MaxLength(500)]
        public string? ProductDescription { get; set; } //รายละเอียดสินค้าอุปกรณ์
        [MaxLength(500)]
        public string? PImages { get; set; } //path image
        [MaxLength(10)]
        public int QtyMinimumStock { get;  set; } //จำนวนที่ต้องแจ้งเตือนเมื่อของเหลือน้อยตามที่กำหนด
        [MaxLength(10)]
        public int QtyInStock { get; set; } //จำนวนนำเข้า
        [Column(TypeName = "decimal(18, 4)")] 
        public decimal UnitPrice { get; set; } //ราคาต่อหน่วย
        [MaxLength(10)]
        public string? UnitOfMeasure { get; set; } // จำนวนชิ้น
        [DataType(DataType.DateTime)]
        public DateTime? ReceiveAt{ get; set; } //วันที่สร้างครั้งแรก
        [DataType (DataType.DateTime)]
        public DateTime? lastAt { get; set;} //update วันที่
    }
}
