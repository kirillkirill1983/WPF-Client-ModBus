using Microsoft.EntityFrameworkCore;

namespace WpfApp4.Model
{
    public class ApplicationContex : DbContext
    {
        public DbSet<DataTimeDB> DataTimeDBs { get; set; }

        public ApplicationContex()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ModbusDB;Trusted_Connection=True;");
        }

    }
}
