using HospitalDbExtended.Core;
using HospitalDbExtended.Data;

namespace HospitalDbExtended
{
    public class Launcher
    {
        public static void Main()
        {
            HospitalContext context = new HospitalContext();

            using (context)
            {
                context.Database.EnsureCreated();

                Engine engine = new Engine(context);
                engine.Run();
            }
        }
    }
}