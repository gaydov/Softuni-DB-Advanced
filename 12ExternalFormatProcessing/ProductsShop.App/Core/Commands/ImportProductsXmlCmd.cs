using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportProductsXmlCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string xmlFilePath = Helpers.TryLocateFileForImport();

            XDocument xDoc = XDocument.Load(xmlFilePath);
            XElement[] productsFromXml = xDoc.Root?.Elements().ToArray();
            ICollection<Product> products = new HashSet<Product>();

            if (productsFromXml != null)
            {
                try
                {
                    foreach (XElement p in productsFromXml)
                    {
                        string name = p.Element("name")?.Value;
                        decimal price = decimal.Parse(p.Element("price")?.Value);

                        Product currentProduct = new Product
                        {
                            Name = name,
                            Price = price
                        };

                        products.Add(currentProduct);
                    }

                    Helpers.AssignRandomSellersAndBuyersToProducts(products, context);

                    context.Products.AddRange(products);
                    context.SaveChanges();

                    string result = string.Format(Messages.SeveralEntitiesImportedFromFile, products.Count, nameof(products),
                        xmlFilePath);

                    if (products.Count == 1)
                    {
                        result = string.Format(Messages.OneEntityImportedFromFile, "product", xmlFilePath);
                    }

                    return result;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException(Messages.InvalidXmlStructure);
                }
            }

            return Messages.XmlEmpty;
        }
    }
}