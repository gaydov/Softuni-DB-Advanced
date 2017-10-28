using System;
using PizzaCalories.Core;
using PizzaCalories.Models;

namespace PizzaCalories
{
    public class Launcher
    {
        public static void Main()
        {
            Engine engine = new Engine();

            try
            {
                engine.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}