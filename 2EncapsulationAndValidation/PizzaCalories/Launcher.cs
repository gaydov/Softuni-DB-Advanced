using System;
using PizzaCalories.Core;

namespace PizzaCalories
{
    public class Launcher
    {
        public static void Main()
        {
            Engine engine = new Engine();
            engine.Run();
        }
    }
}