using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class ConstructionObjectDto
    {
        public ConstructionObjectDto() {}
        public ConstructionObjectDto(ConstructionObject obj)
        {
            Id = obj.Id;
            Name = obj.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
