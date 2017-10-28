using System;
using PizzaCalories.Models;
using PizzaCalories.Utilities;

namespace PizzaCalories.Core
{
    public class Engine
    {
        public void Run()
        {
            string[] pizzaInfo = Console.ReadLine().Split();
            string pizzaName = pizzaInfo[1];

            string[] doughInfo = Console.ReadLine().Split();
            string flourType = doughInfo[1];
            string bakingTechnique = doughInfo[2];
            double doughtWeight = double.Parse(doughInfo[3]);
            Dough dough = new Dough(flourType, bakingTechnique, doughtWeight);
            
            Pizza pizza = new Pizza(pizzaName, dough);
 
            string toppingInfo = Console.ReadLine();
            while (!toppingInfo.Equals("END"))
            {
                string[] args = toppingInfo.Split();
                string toppingType = args[1];
                double toppingWeight = double.Parse(args[2]);
                Topping topping = new Topping(toppingType, toppingWeight);
                pizza.AddTopping(topping);

                toppingInfo = Console.ReadLine();
            }

            if (pizza.ToppingsCount > Constants.PizzaMaxToppingsCount)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PizzaInvalidToppingsCount, Constants.PizzaMinToppingsCount, Constants.PizzaMaxToppingsCount));
            }

            Console.WriteLine($"{pizza.Name} - {pizza.CalcTotalCalories():F2} Calories.");
        }
    }
}