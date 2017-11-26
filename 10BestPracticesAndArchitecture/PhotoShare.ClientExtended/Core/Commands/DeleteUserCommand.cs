using System;
using System.Linq;
using PhotoShare.ClientExtended.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class DeleteUserCommand : Command
    {
        // DeleteUser <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];

            User currentUser = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!Helpers.IsUserTheCurrentlyLoggedOne(currentUser))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            currentUser.IsDeleted = true;
            context.SaveChanges();
            Session.User = null;

            return $"User {currentUser.Username} was deleted successfully!";
        }
    }
}
