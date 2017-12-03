namespace ProductsShop.App.Core
{
    public static class Messages
    {
        public const string PromptFilePathImport = "Please enter file path from which the data to be imported: ";
        public const string PromptFilePathExport = "Please enter file path to which the data to be exported: ";
        public const string FileDoesNotExist = "The specified file does not exist. Please enter a valid path.";
        public const string InvalidJsonStructure = "The specified JSON file is not correctly structured for this command.";
        public const string InvalidXmlStructure = "The specified XML file is not correctly structured for this command.";
        public const string OneEntityImportedFromFile = "1 {0} was imported successfully from \"{1}\".";
        public const string SeveralEntitiesImportedFromFile = "{0} {1} were imported successfully from \"{2}\".";
        public const string InvalidCommand = "Command \"{0}\" not valid!";
        public const string EmptyInputString = "No text entered. Please enter a valid text.";
        public const string InvalidDecimalInput = "Invalid input. Please enter a decimal number.";
        public const string InvalidIntegerInput = "Invalid input. Please enter an integer number.";
        public const string RandomBuyersAndSellersAssigned = "Random categories were assigned to the products in the database.";
        public const string ProductsInRangeExportedToFile = "Products with buyers in price range {0} - {1} were exported to \"{2}\".";
        public const string UsersWithSoldProductsExported = "Users with at least {0} products sold were exported to \"{1}\".";
        public const string CategoriesByProductsExported = "Categories by products count were exported to \"{0}\".";
        public const string MinimumProductsSold = "Minimum products sold: ";
        public const string UserWithProductsExportedToFile = "Users with at least {0} sold products were exported to \"{1}\".";
        public const string XmlEmpty = "The XML file is empty.";
        public const string InvalidBoolInput = "Invalid input. Please enter \"Y\" or \"N\".";
        public const string ExitConfirmation = "Are you sure you want to exit? (Y/N): ";
    }
}