using System.ComponentModel.DataAnnotations;

namespace Warehouse_API.Models
{
    public class ProductType
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(10)]
        public string? TypeID { get; set; }
        [MaxLength(100)]
        public string? TypeName { get; set; }
         
    }
}
