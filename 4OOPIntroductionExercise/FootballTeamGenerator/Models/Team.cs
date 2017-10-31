using System;
using System.Collections.Generic;
using System.Linq;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.Models
{
    public class Team
    {
        private readonly IList<Player> players;
        private string name;

        public Team(string name)
        {
            this.Name = name;
            this.players = new List<Player>();
        }  

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ErrorMessages.InvalidName);
                }

                this.name = value;
            }
        }

        public int Rating
        {
            get
            {
                if (this.players.Count == 0)
                {
                    return 0;
                }

                int rating = (int)Math.Round(this.players.Average(p => p.OverallSkill));
                return rating;
            }
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);
        }

        public void RemovePlayerIfExists(string playerName)
        {
            Player searchedPlayer = this.players.FirstOrDefault(p => p.Name.Equals(playerName));

            if (searchedPlayer == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PlayerNotExisting, playerName, this.Name));
            }

            this.players.Remove(searchedPlayer);
        }
    }
}