using System;
using System.Collections.Generic;
using ProductsShop.Data;
using ProductsShop.Models;

namespace ProductsShop.App.Core.Commands
{
    public class ImportUsersJsonCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            string jsonFilePath = Helpers.TryLocateFileForImport();

            try
            {
                ICollection<User> users = Helpers.ImportFromJson<User>(jsonFilePath);
                context.Users.AddRange(users);
                context.SaveChanges();

                string result = string.Format(Messages.SeveralEntitiesImportedFromFile, users.Count, nameof(users),
                    jsonFilePath);

                if (users.Count == 1)
                {
                    result = string.Format(Messages.OneEntityImportedFromFile, "user", jsonFilePath);
                }

                return result;
            }
            catch (Exception)
            {
                throw new InvalidOperationException(Messages.InvalidJsonStructure);
            }
        }
    }
}