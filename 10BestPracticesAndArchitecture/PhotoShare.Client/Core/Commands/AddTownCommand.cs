using System;
using System.Linq;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class AddTownCommand : Command
    {
        // AddTown <townName> <countryName>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string townName = data[0];
            string country = data[1];

            if (context.Towns.Any(t => t.Name.Equals(townName)))
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            Town town = new Town
            {
                Name = townName,
                Country = country
            };

            context.Towns.Add(town);
            context.SaveChanges();

            return $"Town {townName} was added successfully!";
        }
    }
}
