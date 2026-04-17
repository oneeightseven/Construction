using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IWorkSmetaService
    {
        public Task<List<WorkSmetaDto>> GetSmetaByWorkIdAsync(int workId);
        public Task<int> AddSmetaToWork(AddSmetaToWorkDto model);
        public Task<int> RemoveSmetaById(int smetaId);
    }
}
