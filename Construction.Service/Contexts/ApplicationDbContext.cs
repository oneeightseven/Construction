using Construction.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // DbSet для каждой таблицы
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ConstructionObject> ConstructionObjects { get; set; }
        public DbSet<ShoppingMall> ShoppingMalls { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Work> Works { get; set; }

    }
}
