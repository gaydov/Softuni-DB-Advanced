using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportUsersAndProductsXmlCmd : Command
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

            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("users", new XAttribute("count", usersToSerialize.UsersCount)));

            foreach (var u in usersToSerialize.Users)
            {
                ICollection<XElement> currentUserProducts = new List<XElement>();

                foreach (var p in u.SoldProducts.Products)
                {
                    XElement currentProduct = new XElement("product", new XAttribute("name", p.Name), new XAttribute("price", p.Price));
                    currentUserProducts.Add(currentProduct);
                }

                IList<XAttribute> userAttributes = new List<XAttribute>
                {
                    new XAttribute("last-name", u.LastName)
                };

                if (u.FirstName != null)
                {
                    userAttributes = new List<XAttribute>
                    {
                        new XAttribute("first-name", u.FirstName),
                        new XAttribute("last-name", u.LastName)
                    };
                }

                if (u.Age != null)
                {
                    int age = int.Parse(u.Age.ToString());
                    userAttributes.Add(new XAttribute("age", age));
                }

                XElement userElement = new XElement("user", userAttributes,
                                            new XElement("sold-products", new XAttribute("count", u.SoldProducts.Count),
                                                currentUserProducts));

                xDoc.Root.Add(userElement);
            }

            string xmlFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            StreamWriter writer = new StreamWriter(xmlFilePath);
            using (writer)
            {
                xDoc.Save(writer);
            }

            return string.Format(Messages.UserWithProductsExportedToFile, minimumProductsSold, xmlFilePath);
        }
    }
}