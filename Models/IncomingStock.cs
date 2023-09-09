using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models
{
    //รับเข้าสินค้า
    public class IncomingStock
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]       
        public string? IncomingStockID { get; set; }
        [MaxLength(10)]
        public string? ProductID { get; set; }
        [MaxLength(10)]
        public int QtyReceived { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReceivedDate { get; set; }
        [MaxLength(10)]
        public string? ReceivedBy { get; set; } //userID
    }
}
