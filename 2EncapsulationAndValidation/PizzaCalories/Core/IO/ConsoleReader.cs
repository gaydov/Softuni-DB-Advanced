using System;
using PizzaCalories.Interfaces;

namespace PizzaCalories.Core.IO
{
    public class ConsoleReader : IReader
    {
        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        string IReader.ReadLine()
        {
            return ReadLine();
        }
    }
}