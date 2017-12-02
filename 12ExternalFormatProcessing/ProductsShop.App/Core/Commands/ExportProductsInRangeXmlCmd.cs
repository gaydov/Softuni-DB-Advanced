using System.IO;
using System.Linq;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportProductsInRangeXmlCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string xmlFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            decimal lowestPriceValue = Helpers.TryDecimalParseInputString("Please enter range lowest value: ");
            decimal highestPriceValue = Helpers.TryDecimalParseInputString("Please enter range highest value: ");

            ProductWithBuyerDto[] productsInRange = context.Products
                .Include(p => p.Buyer)
                .Where(p => p.Price >= lowestPriceValue && p.Price <= highestPriceValue && p.Buyer != null)
                .OrderBy(p => p.Price)
                .ProjectTo<ProductWithBuyerDto>()
                .ToArray();

            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("products"));

            foreach (ProductWithBuyerDto p in productsInRange)
            {
                xDoc.Root.Add(new XElement("product", new XAttribute("name", p.Name), new XAttribute("price", p.Price), new XAttribute("buyer", p.Buyer)));
            }

            StreamWriter writer = new StreamWriter(xmlFilePath);
            using (writer)
            {
                xDoc.Save(writer);
            }

            return string.Format(Messages.ProductsInRangeExportedToFile, lowestPriceValue, highestPriceValue, xmlFilePath);
        }
    }
}