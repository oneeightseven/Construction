using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public class DatabaseResetService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMinioCacheService _minioCache;


    private readonly string[] TABLE_NAMES = {"Checks", "Works", "Employees", "Brands", "Cities", "Clients", "ConstructionObjects", "ShoppingMalls", "Statuses", "JobTitle", "Materials", "WorkSmeta"};

    public DatabaseResetService(ApplicationDbContext dbContext, IMinioCacheService minioCache)
    {
        _dbContext = dbContext;
        _minioCache = minioCache;
    }

    public async Task ClearAllTablesAndResetIdentityAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("SET session_replication_role = replica;");
        await _minioCache.ClearBucketAsync();

        foreach (var tableName in TABLE_NAMES)
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{tableName}\" RESTART IDENTITY CASCADE;");
        }

        await _dbContext.Database.ExecuteSqlRawAsync("SET session_replication_role = DEFAULT;");

    }
}