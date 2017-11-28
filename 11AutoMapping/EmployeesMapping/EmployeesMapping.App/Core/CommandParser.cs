using System;
using System.Linq;
using System.Reflection;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Core
{
    public class CommandParser
    {
        private readonly IServiceProvider serviceProvider;

        public CommandParser(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICommand TryParseCommand(string commandName)
        {
            Type[] commandTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            Type currentCommandType =
                commandTypes.SingleOrDefault(t => t.Name.Equals($"{commandName}Command",
                    StringComparison.InvariantCultureIgnoreCase));

            if (currentCommandType == null)
            {
                throw new InvalidOperationException("Invalid command.");
            }

            ConstructorInfo constructor = currentCommandType.GetConstructors()
                .First();

            Type[] constructorParams = constructor.GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            object[] services = constructorParams.Select(this.serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand)constructor.Invoke(services);
            return command;
        }
    }
}