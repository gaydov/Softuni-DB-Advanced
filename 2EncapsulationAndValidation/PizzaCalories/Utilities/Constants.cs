namespace PizzaCalories.Utilities
{
    public static class Constants
    {
        public const string EndOfInputCmd = "END";

        // Dough constants
        public const double DoughBaseCaloriesPerGram = 2;
        public const double DoughMinWeight = 1;
        public const double DoughMaxWeight = 200;

        // Topping constants
        public const double ToppingBaseCaloriesPerGram = 2;
        public const double ToppingMinWeight = 1;
        public const double ToppingMaxWeight = 50;

        // Pizza constants
        public const int PizzaNameMinLenght = 1;
        public const int PizzaNameMaxLenght = 15;
        public const int PizzaMinToppingsCount = 0;
        public const int PizzaMaxToppingsCount = 10;
    }
}