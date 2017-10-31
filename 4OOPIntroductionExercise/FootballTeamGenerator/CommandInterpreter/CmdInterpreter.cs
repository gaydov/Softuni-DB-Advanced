using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FootballTeamGenerator.CommandInterpreter.CommandsModels;
using FootballTeamGenerator.Models;

namespace FootballTeamGenerator.CommandInterpreter
{
    public class CmdInterpreter
    {
        private const string CommandClassSuffix = "Command";

        public Command GenerateCommand(string commandName, string teamName, IList<Team> teams, params string[] additionalParams)
        {
            string fullCommandName = char.ToUpper(commandName[0]) + commandName.Substring(1) + CommandClassSuffix;

            Type commandNameType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == fullCommandName);

            Command command = (Command)Activator.CreateInstance(commandNameType, new object[] { teamName, teams, additionalParams });
            return command;
        }
    }
}