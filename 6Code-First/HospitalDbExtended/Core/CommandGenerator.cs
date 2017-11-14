using System;
using System.Linq;
using System.Reflection;
using HospitalDbExtended.Data;
using HospitalDbExtended.Data.CommandsModels;
using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Core
{
    public class CommandGenerator
    {
        private const string CommandClassSuffix = "Command";

        public Command GenerateCommand(string commandName, HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
        {
            string[] cmdArgs = commandName.Split('-');
            string fullCommandName = string.Empty;

            foreach (string arg in cmdArgs)
            {
                fullCommandName += char.ToUpper(arg[0]) + arg.Substring(1);
            }

            fullCommandName += CommandClassSuffix;

            Type commandNameType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == fullCommandName);

            if (commandNameType == null)
            {
                return null;
            }

            Command command = (Command)Activator.CreateInstance(commandNameType, new object[] { context, isLogged, loggedDoctorId, reader, writer });
            return command;
        }
    }
}