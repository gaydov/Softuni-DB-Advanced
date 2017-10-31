using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FootballTeamGenerator.Enums;
using FootballTeamGenerator.Models;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.CommandInterpreter.CommandsModels
{
    public class AddCommand : Command
    {
        public AddCommand(string teamName, IList<Team> teams, params string[] additionalParams)
            : base(teamName, teams, additionalParams)
        {
        }

        public override void Execute()
        {
            Team currentTeam;
            if (!Helpers.GetTeamIfExists(this.TeamName, this.Teams, out currentTeam))
            {
                throw new ArgumentException(string.Format(ErrorMessages.TeamDoesNotExist, this.TeamName));
            }

            string currentPlayerName = this.AdditionalParams[0];
            IList<Stat> currentPlayerStats = new List<Stat>();

            for (int i = 1; i < this.AdditionalParams.Length; i++)
            {
                Type statType = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == Enum.GetName(typeof(Stats), i));

                Stat stat = (Stat)Activator.CreateInstance(statType, new object[] { int.Parse(this.AdditionalParams[i]) });
                currentPlayerStats.Add(stat);
            }

            Player currentPlayer = new Player(currentPlayerName, currentPlayerStats);
            currentTeam.AddPlayer(currentPlayer);
        }
    }
}