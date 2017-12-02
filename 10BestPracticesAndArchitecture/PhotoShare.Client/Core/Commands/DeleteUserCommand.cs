using System;
using System.Linq;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class DeleteUserCommand : Command
    {
        // DeleteUser <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];

            User user = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (user.IsDeleted != null && user.IsDeleted.Value)
            {
                throw new InvalidOperationException($"User {username} is already deleted!");
            }

            user.IsDeleted = true;
            context.SaveChanges();

            return $"User {user.Username} was deleted successfully!";
        }
    }
}
