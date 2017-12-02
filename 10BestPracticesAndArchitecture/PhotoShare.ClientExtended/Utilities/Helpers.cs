using System;
using PhotoShare.ClientExtended.Core;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Utilities
{
    public static class Helpers
    {
        public static bool IsUserTheCurrentlyLoggedOne(User user)
        {
            return user.Username.Equals(Session.User.Username);
        }
    }
}