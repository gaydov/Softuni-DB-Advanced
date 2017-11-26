using PhotoShare.ClientExtended.Core;
using PhotoShare.Data;

namespace PhotoShare.ClientExtended
{
    public class Application
    {
        public static void Main()
        {
            ResetDatabase();
            CommandDispatcher commandDispatcher = new CommandDispatcher();
            PhotoShareContext context = new PhotoShareContext();

            using (context)
            {
                Engine engine = new Engine(commandDispatcher, context);
                engine.Run();
            }
        }

        private static void ResetDatabase()
        {
            PhotoShareContext context = new PhotoShareContext();

            using (context)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
