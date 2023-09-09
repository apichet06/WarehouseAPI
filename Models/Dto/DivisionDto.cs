using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models.Dto
{
    public class DivisionDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        public string? DV_ID { get; set; }
        [MaxLength(150)]
        public string? DV_Name { get; set; }

    }
}
