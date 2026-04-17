using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class TypeOfAppointmentMapping
    {
        public static List<TypeOfAppointmentDto> Map(List<TypeOfAppointment> models) => models.Select(x => new TypeOfAppointmentDto(x)).ToList();

        public static TypeOfAppointmentDto Map(TypeOfAppointment model) => new(model);
    }
}
