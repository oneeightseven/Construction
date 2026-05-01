using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class AppendicesByWorkIdMapping
    {
        public static List<AppendicesByWorkIdDto> Map(List<StoredFile> files) => files.Select(x => new AppendicesByWorkIdDto(x)).ToList();

        public static AppendicesByWorkIdDto Map(StoredFile file) => new AppendicesByWorkIdDto(file);
    }
}
