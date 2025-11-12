using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IConstructionObjectService
    {
        public Task<List<ConstructionObjectDto>> GetAllAsync();
        Task<string> UpdateAsync(ConstructionObjectDto obj);
        Task<string> DeleteAsync(int id);
    }
}
