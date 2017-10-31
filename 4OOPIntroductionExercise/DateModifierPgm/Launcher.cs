using System;

namespace DateModifierPgm
{
    public class Launcher
    {
        public static void Main()
        {
            string firstDateInput = Console.ReadLine();
            string secondDateInput = Console.ReadLine();
            int differenceInDays = DateModifier.CaclDifferenceBetweenDates(firstDateInput, secondDateInput);

            Console.WriteLine(differenceInDays);
        }
    }
}