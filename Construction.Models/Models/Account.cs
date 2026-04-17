using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TypeOfAppointmentId { get; set; }

        [ForeignKey("TypeOfAppointmentId")]
        public TypeOfAppointment? TypeOfAppointment { get; set; }

        public required DateTime Date { get; set; }

        [Required]
        public int PayerId { get; set; }

        [ForeignKey("PayerId")]
        public Client? Payer { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public Decimal Sum { get; set; }

        [Required]
        public int WorkId { get; set; }

        [ForeignKey("WorkId")]
        public Work? Work { get; set; }
    }
}
