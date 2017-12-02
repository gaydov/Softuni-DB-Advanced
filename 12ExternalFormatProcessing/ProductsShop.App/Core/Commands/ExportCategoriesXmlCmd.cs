using System.IO;
using System.Linq;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExportCategoriesXmlCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            CategoryDto[] categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .ProjectTo<CategoryDto>()
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();

            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("categories"));

            foreach (CategoryDto c in categories)
            {
                xDoc.Root.Add(new XElement("category", new XAttribute("name", c.Category),
                    new XElement("products-count", c.ProductsCount),
                    new XElement("average-price", c.AveragePrice),
                    new XElement("total-revenue", c.TotalRevenue)));
            }

            string xmlFilePath = Helpers.IsNullOrEmptyValidator(Messages.PromptFilePathExport);
            StreamWriter writer = new StreamWriter(xmlFilePath);

            using (writer)
            {
                xDoc.Save(writer);
            }

            return string.Format(Messages.CategoriesByProductsExported, xmlFilePath);
        }
    }
}