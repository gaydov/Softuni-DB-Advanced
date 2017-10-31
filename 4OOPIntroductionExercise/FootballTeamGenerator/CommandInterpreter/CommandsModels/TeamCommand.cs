using System.Collections.Generic;
using FootballTeamGenerator.Models;

namespace FootballTeamGenerator.CommandInterpreter.CommandsModels
{
    public class TeamCommand : Command
    {
        public TeamCommand(string teamName, IList<Team> teams, params string[] additionalParams)
            : base(teamName, teams)
        {
        }

        public override void Execute()
        {
            Team team = new Team(this.TeamName);
            this.Teams.Add(team);
        }
    }
}