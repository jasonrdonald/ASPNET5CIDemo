using Microsoft.Data.Entity;

namespace EmployeeApp.Models
{
    public class EmployeeContext : DbContext
    {
        private static bool _created = false;

        public EmployeeContext()
        {
            if (!_created)
            {
                Database.EnsureCreated();
                _created = true;
            }
        }
        
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(EntityOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryStore();
        }
    }
}