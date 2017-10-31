using System.Collections.Generic;
using System.Linq;
using FootballTeamGenerator.Models;

namespace FootballTeamGenerator.Utilities
{
    public static class Helpers
    {
        public static bool GetTeamIfExists(string teamName, IList<Team> teams, out Team team)
        {
            team = teams.FirstOrDefault(t => t.Name.Equals(teamName));

            if (team == null)
            {
                return false;
            }

            return true;
        }
    }
}