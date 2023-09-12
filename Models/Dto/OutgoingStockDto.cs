using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Warehouse_API.Models.Dto
{
    public class OutgoingStockDto
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
        public DateTime WithdrawalDate { get; set; }
        [MaxLength(10)]
        public string? WithdrawnBy { get; set; } //UserID
        [MaxLength(10)]
        public bool WithdrawnDate { get;} //เวลาอนุมัติ
        [MaxLength(10)]
        public string? ApproveBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ApprovDate { get;set; } // สถานะการอนุมัติ (true หรือ false)
    }
}
