using System;
using System.Linq;
using System.Reflection;
using PhotoShare.Client.Core.Commands;

namespace PhotoShare.Client.Core
{
    public class CommandDispatcher
    {
        private const string CommandClassSuffix = "Command";

        public Command DispatchCommand(string[] commandParameters)
        {
            string inputCommand = commandParameters[0];
            string fullCommandName = inputCommand + CommandClassSuffix;

            Type commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name.Equals(fullCommandName, StringComparison.InvariantCultureIgnoreCase));

            if (commandType == null)
            {
                throw new InvalidOperationException($"Command {inputCommand} not valid!");
            }

            Command command = (Command)Activator.CreateInstance(commandType);

            return command;

            // string result = command.Execute(commandParameters.Skip(1).ToArray(), context);
            // return result;
        }
    }
}
