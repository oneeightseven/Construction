using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class ShoppingMallMapping
    {
        public static List<ShoppingMallDto> Map (List<ShoppingMall> shoppingMalls) => shoppingMalls.Select(x => new ShoppingMallDto(x)).ToList();

        public static ShoppingMallDto Map(ShoppingMall shoppingMall) => new ShoppingMallDto(shoppingMall);
    }
}
