using System;
using System.Collections.Generic;
using PizzaCalories.Utilities;

namespace PizzaCalories.Models
{
    public class Pizza
    {
        private readonly IList<Topping> toppings;
        private readonly Dough dough;
        private string name;

        public Pizza(string name, Dough dough)
        {
            this.Name = name;
            this.toppings = new List<Topping>();
            this.dough = dough;
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value) || value.Length > 15)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.PizzaInvalidName, Constants.PizzaNameMinLenght, Constants.PizzaNameMaxLenght));
                }

                this.name = value;
            }
        }

        public int ToppingsCount
        {
            get
            {
                return this.toppings.Count;
            }
        }

        public void AddTopping(Topping topping)
        {
            this.toppings.Add(topping);
        }

        public double CalcTotalCalories()
        {
            double doughCalories = this.dough.CalcCalogies();
            double toppingsCalories = 0;

            foreach (Topping topping in this.toppings)
            {
                toppingsCalories += topping.CalcCalories();
            }

            return doughCalories + toppingsCalories;
        }
    }
}