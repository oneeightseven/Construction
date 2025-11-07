using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class StatusDto
    {
        public StatusDto() {}
        public StatusDto (Status status)
        {
            Id = status.Id;
            Name = status.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
