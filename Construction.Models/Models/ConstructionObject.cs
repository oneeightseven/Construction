using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("ConstructionObjects")]
    public class ConstructionObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
