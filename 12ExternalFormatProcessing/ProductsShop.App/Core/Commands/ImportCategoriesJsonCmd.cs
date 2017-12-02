using System;
using System.Collections.Generic;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportCategoriesJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string jsonFilePath = Helpers.TryLocateFileForImport();

            try
            {
                ICollection<Category> categories = Helpers.ImportFromJson<Category>(jsonFilePath);
                context.Categories.AddRange(categories);
                context.SaveChanges();

                Helpers.AssignRandomCategoriesToProducts(context);

                string result = string.Format(Messages.SeveralEntitiesImportedFromFile, categories.Count, nameof(categories),
                    jsonFilePath);

                if (categories.Count == 1)
                {
                    result = string.Format(Messages.OneEntityImportedFromFile, "category", jsonFilePath);
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