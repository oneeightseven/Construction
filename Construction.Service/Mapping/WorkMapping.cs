using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class WorkMapping
    {
        public static List<WorkDto> Map(List<Work> works) => works.Select(work => new WorkDto(work)).ToList();

        public static WorkDto Map(Work work) => new(work);
    }
}
