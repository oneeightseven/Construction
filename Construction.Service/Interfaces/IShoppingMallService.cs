using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IShoppingMallService
    {
        Task<List<ShoppingMallDto>> GetAllAsync();
        Task<string> UpdateAsync(ShoppingMallDto obj);
        Task<string> DeleteAsync(int id);
    }
}
