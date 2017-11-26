using System;
using System.Diagnostics;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class LogoutCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            User currentlyLoggedUser = Session.User;

            if (currentlyLoggedUser == null)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            Session.User = null;
            return $"User {currentlyLoggedUser.Username} successfully logged out!";
        }
    }
}