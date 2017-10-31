using System;
using FootballTeamGenerator.Utilities;

namespace FootballTeamGenerator.Models
{
    public abstract class Stat
    {
        private int value;

        protected Stat(int value)
        {
            this.Value = value;
        }

        public int Value
        {
            get
            {
                return this.value;
            }

            protected set
            {
                if (value < Constants.StatMinValue || value > Constants.StatMaxValue)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.InvalidStatValue, this.GetType().Name, Constants.StatMinValue, Constants.StatMaxValue));
                }

                this.value = value;
            }
        }
    }
}