namespace PizzaCalories.Utilities
{
    public static class ErrorMessages
    {
        // Dough messages
        public const string DoughInvalidWeight = "Dough weight should be in the range [{0}..{1}].";
        public const string DoughInvalidType = "Invalid type of dough.";

        // Topping messages
        public const string ToppingInvalidType = "Cannot place {0} on top of your pizza.";
        public const string ToppingInvalidWeight = "{0} weight should be in the range [{1}..{2}].";

        // Pizza messages
        public const string PizzaInvalidName = "Pizza name should be between {0} and {1} symbols.";
        public const string PizzaInvalidToppingsCount = "Number of toppings should be in range [{0}..{1}].";

        // Ingredients messages
        public const string ModifierInvalid = "Such a modifier does not exist.";
    }
}