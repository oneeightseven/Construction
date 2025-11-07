using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IWorkService
    {
        Task<List<WorkDto>> GetAllAsync();
    }
}
