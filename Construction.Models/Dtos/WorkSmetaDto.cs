using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class WorkSmetaDto
    {
        public WorkSmetaDto(){}
        public WorkSmetaDto(WorkSmeta model)
        {
            Id = model.Id;            
            Material = model.Material;            
            Count = model.Count;
            Price = model.Price;
        }
        public int Id { get; set; }
        public Material? Material { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
