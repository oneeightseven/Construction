using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class CityDto
    {
        public CityDto() { }
        public CityDto(City model)
        {
            Id = model.Id;
            Name = model.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
