using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models
{
    public class Position
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)] 
        public string? P_ID { get; set; } //รหัสตำแหน่ง
        [MaxLength(150)]
        public string? P_Name { get; set; } //ชื่อตำแหน่ง
        [MaxLength(10)]
        public string? DV_ID { get; set; } //รหัสแผนก
    }
}
