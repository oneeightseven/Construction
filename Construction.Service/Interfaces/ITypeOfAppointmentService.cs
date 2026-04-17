using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface ITypeOfAppointmentService
    {
        public Task<List<TypeOfAppointmentDto>> GetAllAsync();
    }
}
