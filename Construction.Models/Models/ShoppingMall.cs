using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("ShoppingMalls")]
    public class ShoppingMall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
    }
}
