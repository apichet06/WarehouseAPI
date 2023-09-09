using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models.Dto
{
    //รายการสินค้าที่มีในคลัง
    public class ProductDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength (10)]
        public string? ProductID { get; set; }
        [MaxLength(200)]
        public string? ProductName { get; set; }
        [MaxLength(500)]
        public string? ProductDescription { get; set; }
        [MaxLength(10)]
        public string? QtyMin { get;  set; }
        [MaxLength(10)]
        public int QtyInStock { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal UnitPrice { get; set; }
        [MaxLength(10)]
        public string? UnitOfMeasure { get; set; } // จำนวนชิ้น
        [DataType(DataType.DateTime)]
        public DateTime? ReceiveAt{ get; set; }
        [DataType (DataType.DateTime)]
        public DateTime? lastAt { get; set;}
    }
}
