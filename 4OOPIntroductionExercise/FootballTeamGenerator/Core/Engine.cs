using System;
using System.Collections.Generic;
using System.Linq;
using FootballTeamGenerator.CommandInterpreter;
using FootballTeamGenerator.CommandInterpreter.CommandsModels;
using FootballTeamGenerator.Models;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.Core
{
    public class Engine
    {
        private readonly IList<Team> teams;

        public Engine()
        {
            this.teams = new List<Team>();
        }

        public void Run()
        {
            this.ProcessInput();
        }

        private void ProcessInput()
        {
            string command = Console.ReadLine();
            while (!command.Equals(Constants.EndOfInputCommand))
            {
                string[] commandInfo = command.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                CmdInterpreter interpreter = new CmdInterpreter();
                Command cmd = interpreter.GenerateCommand(commandInfo[0], commandInfo[1], this.teams, commandInfo.Skip(2).ToArray());

                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                command = Console.ReadLine();
            }
        }
    }
}