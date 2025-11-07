using Construction.Models.Models;

namespace Construction.Service.Interfaces
{
    public interface IJobTitleService
    {
        Task<List<JobTitle>> GetAllJobTitlesAsync();
    }
}
