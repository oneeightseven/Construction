using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IStatusService
    {
        Task<List<StatusDto>> GetAllAsync();
    }
}
