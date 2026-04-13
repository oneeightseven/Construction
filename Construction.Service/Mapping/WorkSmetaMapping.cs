using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class WorkSmetaMapping
    {
        public static List<WorkSmetaDto> Map(List<WorkSmeta> smetas) => smetas.Select(x => new WorkSmetaDto(x)).ToList();

        public static WorkSmetaDto Map(WorkSmeta smeta) => new(smeta);
    }
}
