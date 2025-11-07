using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class ClientDto
    {
        public ClientDto(){}
        public ClientDto(Client client)
        {
            Id = client.Id;
            Name = client.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
