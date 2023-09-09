using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models
{
    public class OutgoingStock
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        public string? OutgoingStockID { get; set; }
        [MaxLength (10)]
        public string? ProductID { get; set; }
        [MaxLength(10)]    
        public int QTYWithdrawn { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime WithdrawnDate { get; set; }
        [MaxLength(10)]
        public string? WithdrawnBy { get; set; } //UserID
    }
}
