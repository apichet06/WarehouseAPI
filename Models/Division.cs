using System.ComponentModel.DataAnnotations;

namespace ServiceRepairComputer.Models
{
    public class Division
    {
        [Key]
        public int Id { get; set; } 
        [MaxLength(10)]
        public string? DV_ID { get; set; }
        [MaxLength(150)]
        public string? DV_Name { get; set; }
 
    }
}
