using System;
using System.Linq;
using PhotoShare.ClientExtended.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class LoginCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];
            string password = data[1];

            User currentUser = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            string hashedPassword = PasswordHasher.GenerateHash(password + currentUser.Salt);

            if (!currentUser.Password.Equals(hashedPassword))
            {
                throw new ArgumentException("Invalid username or password!");
            }

            if (currentUser.IsDeleted != null && currentUser.IsDeleted.Value)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (Session.User != null)
            {
                throw new ArgumentException("You should logout first!");
            }

            currentUser.LastTimeLoggedIn = DateTime.Now;
            context.SaveChanges();
            Session.User = currentUser;
            return $"User {currentUser.Username} successfully logged in!";
        }
    }
}