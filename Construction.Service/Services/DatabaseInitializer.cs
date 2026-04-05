using Construction.Models.Models;
using Construction.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var resetService = scope.ServiceProvider.GetRequiredService<DatabaseResetService>();

        bool hasAnyBrand = await dbContext.Brands.AnyAsync(cancellationToken);

        if (!hasAnyBrand)
        {
            await resetService.ClearAllTablesAndResetIdentityAsync();
            await InitializeDatabase(dbContext, cancellationToken);
        }
    }

    private async Task InitializeDatabase(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var jobTitles = new[]
        {
            new JobTitle { Title = "Инженер" },
            new JobTitle { Title = "Техник" },
            new JobTitle { Title = "Мастер" },
            new JobTitle { Title = "Бригадир" },
            new JobTitle { Title = "Прораб" }
        };
        await dbContext.JobTitles.AddRangeAsync(jobTitles, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var employees = new[]
        {
            new Employee { Surname = "Иванов", Name = "Иван", Patronymic = "Иванович", JobTitleId = 1 },
            new Employee { Surname = "Петров", Name = "Петр", Patronymic = "Петрович", JobTitleId = 2 },
            new Employee { Surname = "Сидоров", Name = "Сидор", Patronymic = "Сидорович", JobTitleId = 3 },
            new Employee { Surname = "Козлов", Name = "Алексей", Patronymic = "Алексеевич", JobTitleId = 4 },
            new Employee { Surname = "Соколов", Name = "Дмитрий", Patronymic = "Дмитриевич", JobTitleId = 5 }
        };
        await dbContext.Employees.AddRangeAsync(employees, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var brands = new[]
        {
            new Brand { Name = "MAAG" },
            new Brand { Name = "Bershka" },
            new Brand { Name = "Puma" },
            new Brand { Name = "Reebok" },
            new Brand { Name = "Nike" },
            new Brand { Name = "Asics" },
            new Brand { Name = "Under Armour" }
        };
        await dbContext.Brands.AddRangeAsync(brands, cancellationToken);

        var cities = new[]
        {
            new City { Name = "Москва" },
            new City { Name = "Санкт-Петербург" },
            new City { Name = "Новосибирск" },
            new City { Name = "Екатеринбург" },
            new City { Name = "Казань" },
            new City { Name = "Нижний Новгород" }
        };
        await dbContext.Cities.AddRangeAsync(cities, cancellationToken);

        var clients = new[]
        {
            new Client { Name = "ООО МиниСтрой" },
            new Client { Name = "ИП Строй" },
            new Client { Name = "ЗАО ТехноСервис" },
            new Client { Name = "ООО СтройГарант" },
            new Client { Name = "АО МегаСтрой" }
        };
        await dbContext.Clients.AddRangeAsync(clients, cancellationToken);

        var constructionObjects = new[]
        {
            new ConstructionObject { Name = "Торговый центр А" },
            new ConstructionObject { Name = "Жилой комплекс Б" },
            new ConstructionObject { Name = "Офисное здание В" },
            new ConstructionObject { Name = "Складской комплекс Г" },
            new ConstructionObject { Name = "Парковка Д" }
        };
        await dbContext.ConstructionObjects.AddRangeAsync(constructionObjects, cancellationToken);

        var shoppingMalls = new[]
        {
            new ShoppingMall { Name = "Мега", Address = "ул. Ленина, 1", Contact = "+7(495)123-45-67" },
            new ShoppingMall { Name = "Авиапарк", Address = "Ходынский б-р, 4", Contact = "+7(495)987-65-43" },
            new ShoppingMall { Name = "Европейский", Address = "пл. Киевского вокзала, 2", Contact = "+7(495)555-55-55" },
            new ShoppingMall { Name = "Охотный ряд", Address = "Манежная пл., 1", Contact = "+7(495)111-22-33" },
            new ShoppingMall { Name = "Метрополис", Address = "Ленинградское ш., 16А", Contact = "+7(495)444-55-66" }
        };
        await dbContext.ShoppingMalls.AddRangeAsync(shoppingMalls, cancellationToken);

        var statuses = new[]
        {
            new Status { Name = "Новая" },
            new Status { Name = "В работе" },
            new Status { Name = "Завершена" },
            new Status { Name = "Отменена" },
            new Status { Name = "На проверке" }
        };
        await dbContext.Statuses.AddRangeAsync(statuses, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var works = new[]
        {
            new Work
            {
                DateBid = DateOnly.FromDateTime(DateTime.Now),
                Term = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                CompletionDate = null,
                CityId = 1,
                ShoppingMallId = 1,
                BrandId = 1,
                StatusId = 1,
                ConstructionObjectId = 1,
                ClientId = 1,
                EmployeeId = 1,
                DateOfCreation = DateOnly.FromDateTime(DateTime.Now),
                Summ = 150000
            },
            new Work
            {
                DateBid = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                Term = DateOnly.FromDateTime(DateTime.Now.AddDays(25)),
                CompletionDate = null,
                CityId = 2,
                ShoppingMallId = 2,
                BrandId = 2,
                StatusId = 2,
                ConstructionObjectId = 2,
                ClientId = 2,
                EmployeeId = 2,
                DateOfCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                Summ = 250000
            },
            new Work
            {
                DateBid = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                Term = DateOnly.FromDateTime(DateTime.Now.AddDays(20)),
                CompletionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                CityId = 3,
                ShoppingMallId = 3,
                BrandId = 3,
                StatusId = 3,
                ConstructionObjectId = 3,
                ClientId = 3,
                EmployeeId = 3,
                DateOfCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                Summ = 350000
            },
            new Work
            {
                DateBid = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                Term = DateOnly.FromDateTime(DateTime.Now.AddDays(35)),
                CompletionDate = null,
                CityId = 4,
                ShoppingMallId = 4,
                BrandId = 4,
                StatusId = 1,
                ConstructionObjectId = 4,
                ClientId = 4,
                EmployeeId = 4,
                DateOfCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                Summ = 180000
            },
            new Work
            {
                DateBid = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
                Term = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
                CompletionDate = null,
                CityId = 5,
                ShoppingMallId = 5,
                BrandId = 5,
                StatusId = 4,
                ConstructionObjectId = 5,
                ClientId = 5,
                EmployeeId = 5,
                DateOfCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
                Summ = 420000
            }
        };
        await dbContext.Works.AddRangeAsync(works, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var checks = new[]
        {
            new Check
            {
                Name = "Чек #1",
                Content = "Содержимое чека 1",
                Sum = 5000.50m,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Note = "Оплата материалов",
                TechnicianId = 1,
                FileName = "check1.pdf"
            },
            new Check
            {
                Name = "Чек #2",
                Content = "Содержимое чека 2",
                Sum = 7500.30m,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                Note = "Оплата доставки",
                TechnicianId = 2,
                FileName = "check2.pdf"
            }
        };
        await dbContext.Checks.AddRangeAsync(checks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}