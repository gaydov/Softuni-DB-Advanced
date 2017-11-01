using System;
using PizzaCalories.Core.IO;
using PizzaCalories.Interfaces;
using PizzaCalories.Models;
using PizzaCalories.Utilities;

namespace PizzaCalories.Core
{
    public class Engine
    {
        private readonly IReader reader;
        private readonly IWriter writer;
        private Pizza pizza;

        public Engine()
        {
            this.pizza = null;
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
        }

        public void Run()
        {
            try
            {
                this.ProcessInput(this.reader);
            }
            catch (Exception e)
            {
                this.writer.WriteLine(e.Message);
                return;
            }

            this.PrintOutput(this.writer);
        }

        private void ProcessInput(IReader reader)
        {
            string[] pizzaInfo = reader.ReadLine().Split();
            string pizzaName = pizzaInfo[1];

            string[] doughInfo = reader.ReadLine().Split();
            string flourType = doughInfo[1];
            string bakingTechnique = doughInfo[2];
            double doughWeight = double.Parse(doughInfo[3]);
            Dough dough = new Dough(flourType, bakingTechnique, doughWeight);

            this.pizza = new Pizza(pizzaName, dough);

            string toppingInfo = reader.ReadLine();
            while (!toppingInfo.Equals(Constants.EndOfInputCmd))
            {
                string[] args = toppingInfo.Split();
                string toppingType = args[1];
                double toppingWeight = double.Parse(args[2]);
                Topping topping = new Topping(toppingType, toppingWeight);
                this.pizza.AddTopping(topping);

                toppingInfo = reader.ReadLine();
            }
        }

        private void PrintOutput(IWriter writer)
        {
            writer.WriteLine($"{pizza.Name} - {pizza.CalcTotalCalories():F2} Calories.");
        }
    }
}