using System;
using System.Collections.Generic;
using FootballTeamGenerator.Models;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.CommandInterpreter.CommandsModels
{
    public class RemoveCommand : Command
    {
        public RemoveCommand(string teamName, IList<Team> teams, params string[] additionalParams)
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

            string playerName = this.AdditionalParams[0];
            currentTeam.RemovePlayerIfExists(playerName);
        }
    }
}