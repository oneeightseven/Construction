using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class ShoppingMallDto
    {
        public ShoppingMallDto() { }
        public ShoppingMallDto(ShoppingMall model)
        {
            Id = model.Id;
            Name = model.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
