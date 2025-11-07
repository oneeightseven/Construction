using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Patronymic { get; set; }

        [Required]
        public int JobTitleId { get; set; }

        [ForeignKey("JobTitleId")]
        public JobTitle? JobTitle { get; set; }
    }
}
