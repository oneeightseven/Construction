using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("Checks")]
    public class Check
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [Required]
        public required decimal Sum { get; set; }

        [Required]
        public required DateOnly Date {  get; set; }

        public string Note { get; set; } = string.Empty;

        public bool TrueCheckWithVAT = false;
        public bool TrueCheckWithoutVAT = false;
        public bool FalseCheck = false;

        [Required]
        public int TechnicianId { get; set; }

        [ForeignKey("TechnicianId")]
        public Employee? Technician { get; set; }

        public string? FileName { get; set; }
    }
}
