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
            Address = model.Address;
            Contact = model.Contact;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
