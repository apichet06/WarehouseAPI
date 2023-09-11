using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_API.Models
{
    public class OutgoingStock
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)]
        public string? OutgoingStockID { get; set; }
        [MaxLength (10)]
        public string? ProductID { get; set; }
        [MaxLength(10)]    
        public int QTYWithdrawn { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal UnitPrice { get; set; } //ราคาต่อหน่วย
        [DataType(DataType.DateTime)]
        public DateTime WithdrawnDate { get; set; }
        [MaxLength(10)]
        public string? WithdrawnBy { get; set; } //UserID
    }
}
