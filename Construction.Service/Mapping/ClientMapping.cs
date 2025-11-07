using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public static class ClientMapping
    {
        public static List<ClientDto> Map (List<Client> clients) => clients.Select(x => new ClientDto(x)).ToList();

        public static ClientDto Map(Client client) => new ClientDto(client);
    }
}
