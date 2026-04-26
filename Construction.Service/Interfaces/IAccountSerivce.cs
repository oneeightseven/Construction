using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IAccountSerivce
    {
        public Task<List<AccountDto>> GetByWorkIdAsync(int workId);
        public Task<int> AddAccountToWork(AddAcountToWorkDto model);
        public Task<int> RemoveById(int accountId);
    }
}
