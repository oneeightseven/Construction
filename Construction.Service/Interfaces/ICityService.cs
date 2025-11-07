using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface ICityService
    {
        public Task<List<CityDto>> GetAllAsync();
    }
}
