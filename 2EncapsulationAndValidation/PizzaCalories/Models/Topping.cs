using System;
using PizzaCalories.Utilities;

namespace PizzaCalories.Models
{
    public class Topping
    {
        private string type;
        private double weight;

        public Topping(string type, double weight)
        {
            this.Type = type;
            this.Weight = weight;
        }

        private string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                if (!IngredientsHelper.IsToppingValid(value))
                {
                    throw new ArgumentException(string.Format(ErrorMessages.ToppingInvalidType, value));
                }

                this.type = value;
            }
        }

        private double Weight
        {
            get
            {
                return this.weight;
            }

            set
            {
                if (value < Constants.ToppingMinWeight || value > Constants.ToppingMaxWeight)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.ToppingInvalidWeight, this.Type, Constants.ToppingMinWeight, Constants.ToppingMaxWeight));
                }

                this.weight = value;
            }
        }

        public double CalcCalories()
        {
            double result = this.Weight * Constants.ToppingBaseCaloriesPerGram * IngredientsHelper.GetToppingModifier(this.Type);
            return result;
        }
    }
}