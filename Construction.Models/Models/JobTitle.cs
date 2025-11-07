using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("JobTitle")]
    public class JobTitle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }
    }
}
