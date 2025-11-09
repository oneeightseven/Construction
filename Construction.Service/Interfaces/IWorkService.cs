using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IWorkService
    {
        Task<List<WorkDto>> GetAllAsync();
        Task<List<WorkDto>> GetByDateRange(DateOnly dateFrom, DateOnly dateTo);
        Task<string> UpdateWork(WorkDto work);
    }
}
