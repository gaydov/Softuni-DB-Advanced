using System;
using System.Collections.Generic;
using FootballTeamGenerator.Models;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.CommandInterpreter.CommandsModels
{
    public class RatingCommand : Command
    {
        public RatingCommand(string teamName, IList<Team> teams, params string[] additionalParams)
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

            Console.WriteLine($"{currentTeam.Name} - {currentTeam.Rating}");
        }
    }
}