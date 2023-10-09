using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models.Dto
{
    //รับเข้าสินค้า
    public class IncomingStockDto
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)]
        public string? IncomingStockID { get; set; }
        [MaxLength(10)]
        public string? ProductID { get; set; } 
        [Range(1, 999999999)]
        public int QtyReceived { get; set; }
        [DataType(DataType.DateTime)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPriceReceived { get; set; } //ราคาต่อหน่วย
        public DateTime ReceivedDate { get; set; }
        [MaxLength(10)]
        public string? ReceivedBy { get; set; } //userID
        public ProductDto? Product { get; set; }
        public UsersDto? Users { get; set; }
    }
}
