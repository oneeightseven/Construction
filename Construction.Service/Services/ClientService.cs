using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Construction.Models.Models;

namespace Construction.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            return ClientMapping.Map(clients);
        }

        public async Task<ServiceResult> UpdateAsync(ClientDto clientDto)
        {
            try
            {
                if (clientDto.Id == 0)
                {
                    // ✅ СОЗДАНИЕ - проверяем обязательные поля
                    if (string.IsNullOrWhiteSpace(clientDto.Name))
                        return ServiceResult.Failure("Имя клиента обязательно");

                    var newClient = new Client
                    {
                        Name = clientDto.Name.Trim(),
                        // Добавьте другие поля если есть в модели
                        // Email = clientDto.Email,
                        // Phone = clientDto.Phone,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _context.Clients.AddAsync(newClient);
                }
                else
                {
                    // ✅ ОБНОВЛЕНИЕ - используем AsNoTracking для избежания конфликтов
                    var existingClient = await _context.Clients
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == clientDto.Id);

                    if (existingClient == null)
                        return ServiceResult.NotFound("Клиент не найден");

                    var updatedClient = new Client
                    {
                        Id = clientDto.Id,
                        Name = clientDto.Name.Trim(),
                        // Обновите другие поля
                        // Email = clientDto.Email,
                        // Phone = clientDto.Phone
                    };

                    _context.Clients.Update(updatedClient);
                }

                var saved = await _context.SaveChangesAsync();

                return saved > 0
                    ? ServiceResult.Success()
                    : ServiceResult.Failure("Не удалось сохранить изменения");
            }
            catch (DbUpdateException dbEx)
            {
                // ✅ Подробное логирование ошибок БД
                Console.WriteLine($"❌ DbUpdateException: {dbEx.Message}");
                Console.WriteLine($"📋 Inner: {dbEx.InnerException?.Message}");

                return ServiceResult.Failure($"Ошибка базы данных: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Общая ошибка: {ex.Message}");
                return ServiceResult.Failure($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                // ✅ Используем AsNoTracking для избежания конфликтов
                var client = await _context.Clients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (client == null)
                    return ServiceResult.NotFound("Клиент не найден");

                // ✅ Создаем новый объект только с ID для удаления
                var clientToDelete = new Client { Id = id };
                _context.Clients.Attach(clientToDelete);
                _context.Clients.Remove(clientToDelete);

                var deleted = await _context.SaveChangesAsync();

                return deleted > 0
                    ? ServiceResult.Success("Клиент успешно удален")
                    : ServiceResult.Failure("Не удалось удалить клиента");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"❌ Ошибка удаления: {dbEx.Message}");
                Console.WriteLine($"📋 Inner: {dbEx.InnerException?.Message}");

                // ✅ Проверяем связанные записи
                if (dbEx.InnerException?.Message?.Contains("REFERENCE") == true)
                    return ServiceResult.Failure("Нельзя удалить клиента, так как с ним связаны другие записи");

                return ServiceResult.Failure($"Ошибка базы данных: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Общая ошибка удаления: {ex.Message}");
                return ServiceResult.Failure($"Ошибка: {ex.Message}");
            }
        }
    }

    // ✅ Вспомогательный класс для стандартизированных ответов
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public static ServiceResult Success(string message = "Успешно")
            => new ServiceResult { IsSuccess = true, Message = message };

        public static ServiceResult Failure(string error)
            => new ServiceResult { IsSuccess = false, Error = error };

        public static ServiceResult NotFound(string error = "Не найдено")
            => new ServiceResult { IsSuccess = false, Error = error };
    }
}