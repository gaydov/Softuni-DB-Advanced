using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core
{
    public static class Helpers
    {
        public static ICollection<T> ImportFromJson<T>(string jsonFilePath)
        {
            ICollection<T> deserializedObjects = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(jsonFilePath));

            return deserializedObjects;
        }

        public static bool DoesFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string TryLocateFileForImport()
        {
            Console.Write(Messages.PromptFilePathImport);
            string filePath = Console.ReadLine();

            while (!DoesFileExist(filePath))
            {
                Console.WriteLine(Messages.FileDoesNotExist);
                Console.Write(Messages.PromptFilePathImport);
                filePath = Console.ReadLine();
            }

            return filePath;
        }

        public static string IsNullOrEmptyValidator(string promptMessage)
        {
            Console.Write(promptMessage);
            string inputString = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(inputString))
            {
                Console.WriteLine(Messages.EmptyInputString);
                Console.Write(Environment.NewLine);
                Console.Write(promptMessage);
                inputString = Console.ReadLine();
            }

            return inputString;
        }

        public static decimal TryDecimalParseInputString(string promptMessage)
        {
            bool isEnteredValueDecimal = decimal.TryParse(IsNullOrEmptyValidator(promptMessage), out decimal decimalResult);

            while (!isEnteredValueDecimal)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine(Messages.InvalidDecimalInput);
                isEnteredValueDecimal = decimal.TryParse(IsNullOrEmptyValidator(promptMessage), out decimalResult);
            }

            return decimalResult;
        }

        public static int TryIntegerParseInputString(string promptMessage)
        {
            bool isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out int intResult);

            while (!isEnteredValueInt)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine(Messages.InvalidIntegerInput);
                isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out intResult);
            }

            return intResult;
        }

        public static void AssignRandomSellersAndBuyersToProducts(ICollection<Product> products, ProductsShopContext context)
        {
            int[] usersIds = context.Users.Select(u => u.Id).ToArray();
            Random rand = new Random();

            int iterationCounter = 0;

            foreach (Product p in products)
            {
                int buyerIndex = rand.Next(0, usersIds.Length);
                int sellerIndex = rand.Next(0, usersIds.Length);

                while (buyerIndex == sellerIndex)
                {
                    buyerIndex = rand.Next(0, usersIds.Length);
                    sellerIndex = rand.Next(0, usersIds.Length);
                }

                int? buyerId = usersIds[buyerIndex];
                int sellerId = usersIds[sellerIndex];

                iterationCounter++;

                if (iterationCounter % 3 == 0)
                {
                    buyerId = null;
                }

                p.BuyerId = buyerId;
                p.SellerId = sellerId;
            }
        }

        public static void AssignRandomCategoriesToProducts(ProductsShopContext context)
        {
            int[] productsIds = context.Products.Select(p => p.Id).ToArray();
            int[] categoriesIds = context.Categories.Select(c => c.Id).ToArray();
            Random rand = new Random();
            ICollection<CategoryProduct> cateogoriesProductsForDb = new HashSet<CategoryProduct>();

            foreach (int productId in productsIds)
            {
                int categoryIndex = rand.Next(0, categoriesIds.Length);
                int randomCategoryId = categoriesIds[categoryIndex];

                while (cateogoriesProductsForDb.Any(cp => cp.CategoryId == randomCategoryId && cp.ProductId == productId))
                {
                    categoryIndex = rand.Next(0, categoriesIds.Length);
                    randomCategoryId = categoriesIds[categoryIndex];
                }

                CategoryProduct categoryProduct = new CategoryProduct
                {
                    CategoryId = randomCategoryId,
                    ProductId = productId
                };

                cateogoriesProductsForDb.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(cateogoriesProductsForDb);
            context.SaveChanges();
        }

        public static bool ValidateBoolEntered(string promptMessage)
        {
            Console.Write(promptMessage);
            string inputString = Console.ReadLine();

            while (!inputString.ToLower().Equals("y") && !inputString.ToLower().Equals("n"))
            {
                Console.WriteLine(Messages.InvalidBoolInput);
                Console.Write(Environment.NewLine);
                Console.Write(promptMessage);
                inputString = Console.ReadLine();
            }

            bool result = inputString.ToLower().Equals("y");
            return result;
        }
    }
}