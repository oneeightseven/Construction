using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IMaterialSerivce
    {
        public Task<List<MaterialDto>> GetAllAsync();
    }
}
