using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class ConstructionObjectMapping
    {
        public static List<ConstructionObjectDto> Map(List<ConstructionObject> objects) => objects.Select(obj => new ConstructionObjectDto(obj)).ToList();

        public static ConstructionObjectDto Map(ConstructionObject obj) => new(obj);
    }
}
