using System.IO;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportProductsInRangeJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string jsonFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            decimal lowestPriceValue = Helpers.TryDecimalParseInputString("Please enter range lowest value: ");
            decimal highestPriceValue = Helpers.TryDecimalParseInputString("Please enter range highest value: ");

            ProductWithSellerDto[] productsInRange = context.Products
                .Include(p => p.Seller)
                .Where(p => p.Price >= lowestPriceValue && p.Price <= highestPriceValue)
                .OrderBy(p => p.Price)
                .ProjectTo<ProductWithSellerDto>()
                .ToArray();

            string serializedProductsInRange = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);

            File.WriteAllText(jsonFilePath, serializedProductsInRange);

            return string.Format(Messages.ProductsInRangeExportedToFile, lowestPriceValue, highestPriceValue, jsonFilePath);
        }
    }
}