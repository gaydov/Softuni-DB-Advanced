using System;
using System.Linq;
using System.Reflection;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class ModifyUserCommand : Command
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];
            string property = data[1];
            string propertyNewValue = data[2];

            User currentUser = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            PropertyInfo[] userProperties = currentUser.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!userProperties.Any(p => p.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException($"Property {property} not supported!");
            }

            switch (property.ToLower())
            {
                case "password":
                    if (!propertyNewValue.Any(char.IsLower) || !propertyNewValue.Any(char.IsDigit))
                    {
                        throw new ArgumentException($"Value {propertyNewValue} not valid.{Environment.NewLine}Invalid Password");
                    }

                    currentUser.Salt = PasswordHasher.GenerateSalt();
                    currentUser.Password = PasswordHasher.GenerateHash(propertyNewValue + currentUser.Salt);
                    break;

                case "borntown":
                    Town newBornTown = context.Towns.SingleOrDefault(t => t.Name.Equals(propertyNewValue));

                    if (newBornTown == null)
                    {
                        throw new ArgumentException($"Value {propertyNewValue} not valid.{Environment.NewLine}Town {propertyNewValue} not found!");
                    }

                    currentUser.BornTown = newBornTown;
                    break;

                case "currenttown":
                    Town newCurrentTown = context.Towns.SingleOrDefault(t => t.Name.Equals(propertyNewValue));

                    if (newCurrentTown == null)
                    {
                        throw new ArgumentException($"Value {propertyNewValue} not valid.{Environment.NewLine}Town {propertyNewValue} not found!");
                    }

                    currentUser.CurrentTown = newCurrentTown;
                    break;

                default:
                    return $"Property {property} not supported!";
            }

            context.SaveChanges();

            return $"User {username} {property} is {propertyNewValue}.";
        }
    }
}
