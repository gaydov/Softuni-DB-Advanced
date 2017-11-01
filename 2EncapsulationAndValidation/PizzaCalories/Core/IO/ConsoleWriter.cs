using System;
using PizzaCalories.Interfaces;

namespace PizzaCalories.Core.IO
{
    public class ConsoleWriter : IWriter
    {
        public static void WriteLine(string textLine)
        {
            Console.WriteLine(textLine);
        }

        void IWriter.WriteLine(string textLine)
        {
            WriteLine(textLine);
        }
    }
}