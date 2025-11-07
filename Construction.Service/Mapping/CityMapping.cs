using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public class CityMapping
    {
        public static List<CityDto> Map(List<City> cities) => cities.Select(x => new CityDto(x)).ToList();

        public static CityDto Map(City city) => new CityDto(city);
    }
}
