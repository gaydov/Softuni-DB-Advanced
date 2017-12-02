using System;
using System.Linq;
using System.Reflection;
using ProductsShop.App.Core.Commands;

namespace ProductsShop.App.Core
{
    public class CommandInterpreter
    {
        private const string CommandClassSuffix = "Cmd";

        public Command TryInterpretCommand(string inputCommand)
        {
            string inputCommandWithoutDashes = inputCommand.Replace("-", string.Empty);
            string fullCommandName = inputCommandWithoutDashes + CommandClassSuffix;

            Type commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name.Equals(fullCommandName, StringComparison.InvariantCultureIgnoreCase));

            if (commandType == null)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, inputCommand));
            }

            Command command = (Command)Activator.CreateInstance(commandType);
            return command;
        }
    }
}