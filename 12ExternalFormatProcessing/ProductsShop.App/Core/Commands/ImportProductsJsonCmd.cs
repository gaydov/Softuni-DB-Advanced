using System;
using System.Collections.Generic;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportProductsJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string jsonFilePath = Helpers.TryLocateFileForImport();

            try
            {
                ICollection<Product> products = Helpers.ImportFromJson<Product>(jsonFilePath);
                Helpers.AssignRandomSellersAndBuyersToProducts(products, context);

                context.Products.AddRange(products);
                context.SaveChanges();

                string result = string.Format(Messages.SeveralEntitiesImportedFromFile, products.Count, nameof(products),
                    jsonFilePath);

                if (products.Count == 1)
                {
                    result = string.Format(Messages.OneEntityImportedFromFile, "product", jsonFilePath);
                }

                return result;
            }
            catch (Exception)
            {
                throw new InvalidOperationException(Messages.InvalidJsonStructure);
            }
        }
    }
}