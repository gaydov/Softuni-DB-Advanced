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

        public Command GenerateCommand(string command, HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
        {
            string[] cmdArgs = command.Split('-');
            string fullCommandName = string.Empty;

            foreach (string arg in cmdArgs)
            {
                fullCommandName += char.ToUpper(arg[0]) + arg.Substring(1);
            }

            fullCommandName += CommandClassSuffix;

            Type commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == fullCommandName);

            if (commandType == null)
            {
                return null;
            }

            Command resultCommand = (Command)Activator.CreateInstance(commandType, new object[] { context, isDoctorLogged, loggedDoctorId, reader, writer });
            return resultCommand;
        }
    }
}