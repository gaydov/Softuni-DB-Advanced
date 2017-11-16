using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    public class Launcher
    {
        public static void Main()
        {
            FootballBettingContext context = new FootballBettingContext();

            using (context)
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
        }
    }
}
