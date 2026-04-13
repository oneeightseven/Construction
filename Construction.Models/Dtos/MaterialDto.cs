using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class MaterialDto
    {
        public MaterialDto(){}
        public MaterialDto(Material model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Price = model.Price;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
