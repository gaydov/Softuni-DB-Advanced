using System.IO;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportCategoriesJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            CategoryDto[] categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .OrderBy(c => c.Name)
                .ProjectTo<CategoryDto>()
                .ToArray();

            string serializedCategories = JsonConvert.SerializeObject(categories, Formatting.Indented);
            string jsonFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            File.WriteAllText(jsonFilePath, serializedCategories);

            return string.Format(Messages.CategoriesByProductsExported, jsonFilePath);
        }
    }
}