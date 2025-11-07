using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class StatusMapping
    {
        public static List<StatusDto> Map(List<Status> statuses) => statuses.Select(status => new StatusDto(status)).ToList();

        public static StatusDto Map(Status status) => new(status);
    }
}
