using P01_BillsPaymentSystem.Core;
using P01_BillsPaymentSystem.Data;

namespace P01_BillsPaymentSystem
{
    public class Launcher
    {
        public static void Main()
        {
            BillsPaymentSystemContext context = new BillsPaymentSystemContext();

            using (context)
            {
                Engine engine = new Engine(context);
                engine.Run();
            }
        }
    }
}
