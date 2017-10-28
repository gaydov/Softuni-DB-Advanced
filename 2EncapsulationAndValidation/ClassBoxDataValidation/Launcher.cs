using System;
using System.Linq;
using System.Reflection;

namespace ClassBoxDataValidation
{
    public class Launcher
    {
        public static void Main()
        {
            Type boxType = typeof(Box);
            FieldInfo[] fields = boxType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine(fields.Count());

            double inputLength = double.Parse(Console.ReadLine());
            double inputWidth = double.Parse(Console.ReadLine());
            double inputHeight = double.Parse(Console.ReadLine());

            try
            {
                Box box = new Box(inputLength, inputWidth, inputHeight);

                Console.WriteLine($"Surface Area - {box.CalcSurfaceArea():F2}");
                Console.WriteLine($"Lateral Surface Area - {box.CalcLaterialSurfaceArea():F2}");
                Console.WriteLine($"Volume - {box.CalcVolume():F2}");
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}