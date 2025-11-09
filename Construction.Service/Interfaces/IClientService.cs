using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IClientService
    {
        public Task<List<ClientDto>> GetAllAsync();
        Task<string> UpdateAsync(ClientDto client);
        Task<string> DeleteAsync(int id);
    }
}
