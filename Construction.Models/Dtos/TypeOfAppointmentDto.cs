using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class TypeOfAppointmentDto
    {
        public TypeOfAppointmentDto() {}
        public TypeOfAppointmentDto(TypeOfAppointment model)
        {
            Id = model.Id;
            Name = model.Name;
        }
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
