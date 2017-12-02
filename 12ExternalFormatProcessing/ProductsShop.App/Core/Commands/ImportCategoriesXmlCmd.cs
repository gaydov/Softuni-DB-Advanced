using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportCategoriesXmlCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string xmlFilePath = Helpers.TryLocateFileForImport();

            XDocument xDoc = XDocument.Load(xmlFilePath);
            XElement[] categoriesFromXml = xDoc.Root?.Elements().ToArray();
            ICollection<Category> categories = new HashSet<Category>();

            if (categoriesFromXml != null)
            {
                try
                {
                    foreach (XElement c in categoriesFromXml)
                    {
                        string name = c.Element("name")?.Value;

                        Category currentCategory = new Category
                        {
                            Name = name
                        };

                        categories.Add(currentCategory);
                    }

                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                    
                    Helpers.AssignRandomCategoriesToProducts(context);

                    string result = string.Format(Messages.SeveralEntitiesImportedFromFile, categories.Count, nameof(categories),
                        xmlFilePath);

                    if (categories.Count == 1)
                    {
                        result = string.Format(Messages.OneEntityImportedFromFile, "category", xmlFilePath);
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