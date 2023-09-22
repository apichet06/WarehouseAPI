using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models
{
    //รับเข้าสินค้า
    public class IncomingStock
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)]       
        public string? IncomingStockID { get; set; }
        [MaxLength(10)]
        public string? ProductID { get; set; }
        [Range(1, 999999999)]
        public int QtyReceived { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPriceReceived { get; set; } //ราคาต่อหน่วย
        [DataType(DataType.DateTime)]
        public DateTime ReceivedDate { get; set; }
        [MaxLength(10)]
        public string? ReceivedBy { get; set; } //userID
    }
}
