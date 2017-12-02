using System;
using System.Linq;
using PhotoShare.ClientExtended.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class RegisterUserCommand : Command
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];
            string password = data[1];
            string repeatPassword = data[2];
            string email = data[3];

            if (context.Users.Any(u => u.Username == username))
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if (!password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                throw new ArgumentException($"Value {password} not valid. Password must include a lower letter and a digit.{Environment.NewLine}Invalid Password");
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            string salt = PasswordHasher.GenerateSalt();

            User currentUser = new User
            {
                Username = username,
                Salt = salt,
                Password = PasswordHasher.GenerateHash(password + salt),
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now
            };

            context.Users.Add(currentUser);
            context.SaveChanges();

            return $"User {currentUser.Username} was registered successfully!";
        }
    }
}
