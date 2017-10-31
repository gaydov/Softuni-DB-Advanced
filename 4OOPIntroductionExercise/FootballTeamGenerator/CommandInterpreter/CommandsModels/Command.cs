using System.Collections.Generic;
using FootballTeamGenerator.Models;

namespace FootballTeamGenerator.CommandInterpreter.CommandsModels
{
    public abstract class Command
    {
        private string teamName;
        private IList<Team> teams;
        private string[] additionalParams;

        protected Command(string teamName, IList<Team> teams, params string[] additionalParams)
        {
            this.teamName = teamName;
            this.teams = teams;
            this.additionalParams = additionalParams;
        }

        protected string TeamName
        {
            get
            {
                return this.teamName;
            }

            set
            {
                this.teamName = value;
            }
        }

        protected IList<Team> Teams
        {
            get
            {
                return this.teams;
            }

            set
            {
                this.teams = value;
            }
        }

        protected string[] AdditionalParams
        {
            get
            {
                return this.additionalParams;
            }

            set
            {
                this.additionalParams = value;
            }
        }

        public abstract void Execute();
    }
}