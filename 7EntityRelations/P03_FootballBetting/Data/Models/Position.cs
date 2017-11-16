using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        public int PositionId { get; set; }

        public string Name { get; set; }

        public ICollection<Player> Players { get; set; }    
    }
}