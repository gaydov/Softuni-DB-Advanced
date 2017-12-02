using System;
using System.Linq;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesMapping.App.Core
{
    public class Engine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            IDbInitializerService dbInitializerService = this.serviceProvider.GetService<IDbInitializerService>();
            dbInitializerService.InitializeDatabase();

            CommandParser cmdParser = new CommandParser(this.serviceProvider);

            while (true)
            {
                Console.Write("Enter command: ");

                string[] input = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string commandName = input[0];
                string[] commandArgs = input.Skip(1).ToArray();

                try
                {
                    ICommand command = cmdParser.TryParseCommand(commandName);
                    string result = command.Execute(commandArgs);

                    Console.WriteLine(result);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Invalid command.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}