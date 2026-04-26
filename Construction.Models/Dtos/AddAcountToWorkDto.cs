
namespace Construction.Models.Dtos
{
    public class AddAcountToWorkDto
    {
        public int? Id { get; set; }
        public string? Details { get; set; }
        public int TypeOfAppointmentId { get; set; }
        public int PayerId { get; set; }
        public decimal Sum { get; set; }
        public int WorkId { get; set; }
        public DateTime Date { get; set; }
    }
}
