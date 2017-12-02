using System.IO;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportSoldProductsJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            int minimumSoldProductsCount = Helpers.TryIntegerParseInputString("Minimum sold products count: ");

            UserWithSoldProductsDto[] usersSoldAtLeastOneProduct = context.Users
                .Include(u => u.ProductsSold)
                .ThenInclude(p => p.Buyer)
                .Where(u => u.ProductsSold.Count >= minimumSoldProductsCount && u.ProductsSold.All(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UserWithSoldProductsDto>()
                .ToArray();

            string serializedUsersSoldAtLeastOneProduct = JsonConvert.SerializeObject(usersSoldAtLeastOneProduct,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            string jsonFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            File.WriteAllText(jsonFilePath, serializedUsersSoldAtLeastOneProduct);

            return string.Format(Messages.UsersWithSoldProductsExported, minimumSoldProductsCount, jsonFilePath);
        }
    }
}