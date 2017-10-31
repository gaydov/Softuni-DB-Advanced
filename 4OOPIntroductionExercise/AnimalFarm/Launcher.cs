using System;
using AnimalFarm.Models;

namespace AnimalFarm
{
    public class Launcher
    {
        public static void Main(string[] args)
        {
            string name = Console.ReadLine();
            int age = int.Parse(Console.ReadLine());

            try
            {
                Chicken chicken = new Chicken(name, age);

                Console.WriteLine(
                    "Chicken {0} (age {1}) can produce {2} eggs per day.",
                    chicken.Name,
                    chicken.Age,
                    chicken.GetProductPerDay());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}