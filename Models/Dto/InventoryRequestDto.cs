using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models.Dto
{
    public class InventoryRequestDto
    { 
            [Key]
            public int ID { get; set; } // รหัส id (Primary Key) 
            [MaxLength(30)]
            public string? RequestCode { get; set; }  // รหัส orderId RequestCode
            [DataType(DataType.DateTime)]
            public DateTime TransactionTime { get; set; } // เวลาเบิก
            [MaxLength(10)]
            public string? RequesterUserId { get; set; } // ชื่อผู้เบิก userId
            [MaxLength(10)]
            public string? DivisionId { get; set; } // แผนกที่เบิก divisionId
            [MaxLength(10)]
            public string? ApproveBy { get; set; }// ชื่อผู้อนุมัติสินค้า userId
            [DataType(DataType.DateTime)]
            public DateTime? AppvDate { get; set; } // เวลาอนุมัติ 
            [MaxLength(500)]
            public string? Purpose { get; set; } // วัตถุประสงค์ในการเบิก
            [MaxLength(1)]
            public string? IsApproved { get; set; } = "i"; //status การอนุมัติ
            [MaxLength(400)]
            public string? Note { get; set; }
            public UsersDto? Users { get; set; } 
            public UsersDto? ApprovedUsers { get; set; }
    }
     
}