using System;
using PizzaCalories.Utilities;

namespace PizzaCalories.Models
{
    public class Dough
    {
        private string flourType;
        private string bakingTechnique;
        private double weightGrams;

        public Dough(string flourType, string bakingTechnique, double weightGrams)
        {
            this.FlourType = flourType;
            this.BakingTechnique = bakingTechnique;
            this.WeightGrams = weightGrams;
        }

        private string FlourType
        {
            set
            {
                if (!Ingredients.IsDoughValid(value))
                {
                    throw new ArgumentException(ErrorMessages.DoughInvalidType);
                }

                this.flourType = value;
            }
        }

        private string BakingTechnique
        {
            set
            {
                if (!Ingredients.IsDoughValid(value))
                {
                    throw new ArgumentException(ErrorMessages.DoughInvalidType);
                }

                this.bakingTechnique = value;
            }
        }

        private double WeightGrams
        {
            get
            {
                return this.weightGrams;
            }

            set
            {
                if (value < Constants.DoughMinWeight || value > Constants.DoughMaxWeight)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.DoughInvalidWeight, Constants.DoughMinWeight, Constants.DoughMaxWeight));
                }

                this.weightGrams = value;
            }
        }

        public double CalcCalogies()
        {
            double result = this.WeightGrams * Constants.DoughBaseCaloriesPerGram * Ingredients.GetDoughModifier(this.flourType) * Ingredients.GetDoughModifier(this.bakingTechnique);
            return result;
        }
    }
}