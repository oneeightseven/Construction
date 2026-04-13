using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("WorkSmeta")]
    public class WorkSmeta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [ForeignKey("MaterialId")]
        public Material? Material { get; set; }

        [Required]
        public int WorkId { get; set; }

        [ForeignKey("WorkId")]
        public Work? Work { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
