using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportUsersXmlCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string xmlFilePath = Helpers.TryLocateFileForImport();

            XDocument xDoc = XDocument.Load(xmlFilePath);
            XElement[] usersFromXml = xDoc.Root?.Elements().ToArray();
            ICollection<User> users = new HashSet<User>();

            if (usersFromXml != null)
            {
                try
                {
                    foreach (XElement u in usersFromXml)
                    {
                        string firstName = u.Attribute("firstName")?.Value;
                        string lastName = u.Attribute("lastName")?.Value;
                        int? age = null;

                        if (u.Attribute("age")?.Value != null)
                        {
                            age = int.Parse(u.Attribute("age").Value);
                        }

                        User currentUser = new User
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Age = age
                        };

                        users.Add(currentUser);
                    }

                    context.Users.AddRange(users);
                    context.SaveChanges();

                    string result = string.Format(Messages.SeveralEntitiesImportedFromFile, users.Count, nameof(users),
                        xmlFilePath);

                    if (users.Count == 1)
                    {
                        result = string.Format(Messages.OneEntityImportedFromFile, "user", xmlFilePath);
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