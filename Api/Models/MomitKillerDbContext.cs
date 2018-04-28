using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace MomitKiller.Api.Models
{
    public class MomitKillerDbContext:DbContext
    {
        private string _connectionString;

        public DbSet<ThermostatConfig> ThermostatConfig
        {
            get;
            set;
        }

        public DbSet<ThermostatConfig> Temperature
        {
            get;
            set;
        }


        public MomitKillerDbContext()
        {
            _connectionString = "Data Source=MomitKiller.db";
        }

        public MomitKillerDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MomitKillerDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString ?? "Data Source=MomitKiller.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
