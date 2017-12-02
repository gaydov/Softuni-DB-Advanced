using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class ListFriendsCommand : Command
    {
        // PrintFriendsList <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];

            User currentUser = context
                .Users
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.User)
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (currentUser.AddedAsFriendBy.Count == 0)
            {
                return "No friends for this user. :(";
            }

            StringBuilder sb = new StringBuilder();

            foreach (Friendship friendship in currentUser.AddedAsFriendBy)
            {
                sb.AppendLine($"-{friendship.User.Username}");
            }

            return sb.ToString().Trim();
        }
    }
}
