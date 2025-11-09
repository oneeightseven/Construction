using Construction.Models.Dtos;

namespace Construction.Service.Interfaces
{
    public interface IExcelHelper
    {
        public MemoryStream GenerateDetailing(List<WorkDto> works);
    }
}
