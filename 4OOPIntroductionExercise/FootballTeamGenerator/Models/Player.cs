using System;
using System.Collections.Generic;
using System.Linq;
using FootballTeamGenerator.Models.StatsModels;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.Models
{
    public class Player
    {
        private readonly IList<Stat> stats;
        private string name;
        
        public Player(string name, IList<Stat> stats)
        {
            this.Name = name;
            this.stats = stats;
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

        public int Endurance
        {
            get
            {
                Stat enduranceStat = this.stats.FirstOrDefault(s => s.GetType() == typeof(Endurance));
                return enduranceStat.Value;
            }
        }

        public int Sprint
        {
            get
            {
                Stat sprintStat = this.stats.FirstOrDefault(s => s.GetType() == typeof(Sprint));
                return sprintStat.Value;
            }
        }

        public int Dribble
        {
            get
            {
                Stat dribbleStat = this.stats.FirstOrDefault(s => s.GetType() == typeof(Dribble));
                return dribbleStat.Value;
            }
        }

        public int Passing
        {
            get
            {
                Stat passingStat = this.stats.FirstOrDefault(s => s.GetType() == typeof(Passing));
                return passingStat.Value;
            }
        }

        public int Shooting
        {
            get
            {
                Stat shootingStat = this.stats.FirstOrDefault(s => s.GetType() == typeof(Shooting));
                return shootingStat.Value;
            }
        }

        public int OverallSkill
        {
            get
            {
                int overallSkill = (int)Math.Round(this.stats.Average(s => s.Value));
                return overallSkill;
            }
        }
    }
}