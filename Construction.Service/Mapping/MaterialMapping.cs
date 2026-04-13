using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class MaterialMapping
    {
        public static List<MaterialDto> Map(List<Material> objects) => objects.Select(x => new MaterialDto(x)).ToList();

        public static MaterialDto Map(Material obj) => new(obj);
    }
}
