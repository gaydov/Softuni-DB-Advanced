using System;
using AutoMapper;
using Employees.Data;
using Employees.Services;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Core;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesMapping.App
{
    public class Launcher
    {
        public static void Main()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            Engine engine = new Engine(serviceProvider);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>();

            serviceCollection.AddTransient<IDbInitializerService, DbInitializerService>();
            serviceCollection.AddTransient<IEmployeeService, EmployeeService>();
            serviceCollection.AddAutoMapper();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
