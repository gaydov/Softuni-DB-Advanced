using System;
using System.Collections.Generic;

namespace PizzaCalories.Utilities
{
    public static class IngredientsHelper
    {
        private static readonly IDictionary<string, double> DoughsWithModifiers = new Dictionary<string, double>
        {
            { "white", 1.5 },
            { "wholegrain", 1.0 },
            { "crispy", 0.9 },
            { "chewy", 1.1 },
            { "homemade", 1.0 }
        };

        private static readonly IDictionary<string, double> ToppingsWithModifiers = new Dictionary<string, double>
        {
            { "meat", 1.2 },
            { "veggies", 0.8 },
            { "cheese", 1.1 },
            { "sauce", 0.9 }
        };

        public static bool IsDoughValid(string doughType)
        {
            string searcherDoughType = doughType.ToLower();

            if (DoughsWithModifiers.ContainsKey(searcherDoughType))
            {
                return true;
            }

            return false;
        }

        public static bool IsToppingValid(string toppingType)
        {
            string searchedTopping = toppingType.ToLower();

            if (ToppingsWithModifiers.ContainsKey(searchedTopping))
            {
                return true;
            }

            return false;
        }

        public static double GetDoughModifier(string flourTypeOrBakingTechnique)
        {
            string searcherDoughModifier = flourTypeOrBakingTechnique.ToLower();

            if (DoughsWithModifiers.ContainsKey(searcherDoughModifier))
            {
                double doughModifier = DoughsWithModifiers[searcherDoughModifier];
                return doughModifier;
            }

            throw new ArgumentException(ErrorMessages.ModifierInvalid);
        }

        public static double GetToppingModifier(string toppingName)
        {
            string searchedToppingModifier = toppingName.ToLower();

            if (ToppingsWithModifiers.ContainsKey(searchedToppingModifier))
            {
                double toppingModifier = ToppingsWithModifiers[searchedToppingModifier];
                return toppingModifier;
            }

            throw new ArgumentException(ErrorMessages.ModifierInvalid);
        }
    }
}