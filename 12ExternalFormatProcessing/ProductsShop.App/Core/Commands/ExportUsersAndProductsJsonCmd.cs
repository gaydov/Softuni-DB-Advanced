using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportUsersAndProductsJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
           int minimumProductsSold = Helpers.TryIntegerParseInputString(Messages.MinimumProductsSold);

            var users = context.Users
                .Include(u => u.ProductsSold)
                .Where(u => u.ProductsSold.Count >= minimumProductsSold && u.ProductsSold.All(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                    }
                })
                .ToArray();

            var usersToSerialize = new
            {
                UsersCount = users.Length,
                Users = users
            };

            string serializedUsersWithSoldProducts =
                JsonConvert.SerializeObject(usersToSerialize,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            string jsonFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            File.WriteAllText(jsonFilePath, serializedUsersWithSoldProducts);

            return string.Format(Messages.UserWithProductsExportedToFile, minimumProductsSold, jsonFilePath);
        }
    }
}