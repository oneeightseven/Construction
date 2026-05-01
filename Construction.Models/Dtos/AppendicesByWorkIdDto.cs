using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class AppendicesByWorkIdDto
    {
        public AppendicesByWorkIdDto() { }

        public AppendicesByWorkIdDto(StoredFile file)
        {
            Id = file.Id;
            FileName = file.StoredFileName;
            Date = file.CreatedAt;
            ContentType = file.ContentType;
            OriginalFileName = file.OriginalFileName;
        }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public DateTime Date { get; set; }
    }
}
