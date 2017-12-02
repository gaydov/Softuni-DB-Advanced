using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportSoldProductsXmlCmd : Command
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

            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("users"));

            foreach (UserWithSoldProductsDto u in usersSoldAtLeastOneProduct)
            {
                ICollection<XElement> currentUserProducts = new List<XElement>();

                foreach (SoldProductDto p in u.SoldProducts)
                {
                    XElement currentProduct = new XElement("product",
                                                    new XElement("name", p.Name),
                                                    new XElement("price", p.Price));

                    currentUserProducts.Add(currentProduct);
                }

                XAttribute[] names =
                {
                    new XAttribute("last-name", u.LastName)
                };

                if (u.FirstName != null)
                {
                    names = new[]
                    {
                        new XAttribute("first-name", u.FirstName),
                        new XAttribute("last-name", u.LastName)
                    };
                }

                XElement userElement = new XElement("user", names,
                                            new XElement("sold-products",
                                                currentUserProducts));

                xDoc.Root.Add(userElement);
            }

            string xmlFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            StreamWriter writer = new StreamWriter(xmlFilePath);

            using (writer)
            {
                xDoc.Save(writer);
            }

            return $"Users with at least {minimumSoldProductsCount} products sold were exported to \"{xmlFilePath}\".";
        }
    }
}