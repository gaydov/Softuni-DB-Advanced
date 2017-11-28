using Employees.Data;
using Employees.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services
{
    public class DbInitializerService : IDbInitializerService
    {
        private readonly EmployeesContext context;

        public DbInitializerService(EmployeesContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.EnsureDeleted();
            this.context.Database.Migrate();
        }
    }
}