using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IClientService
    {
        public Task<List<ClientDto>> GetAll();

    }
}
