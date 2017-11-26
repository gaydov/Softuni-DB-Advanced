using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.ClientExtended.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class AddFriendCommand : Command
    {
        // AddFriend <username1> <username2>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string senderUsername = data[0];
            string receiverUsername = data[1];

            User sender = context
                .Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username.Equals(senderUsername));

            if (sender == null)
            {
                throw new ArgumentException($"User {senderUsername} not found!");
            }

            if (!Helpers.IsUserTheCurrentlyLoggedOne(sender))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            User receiver = context
                .Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username.Equals(receiverUsername));

            if (receiver == null)
            {
                throw new ArgumentException($"User {receiverUsername} not found!");
            }

            if (sender.FriendsAdded.Any(f => f.Friend.Username.Equals(receiverUsername)))
            {
                throw new InvalidOperationException($"{receiverUsername} is already a friend to {senderUsername}!");
            }

            Friendship friendship = new Friendship
            {
                User = sender,
                Friend = receiver
            };

            sender.FriendsAdded.Add(friendship);
            context.SaveChanges();

            return $"Friend {receiverUsername} added to {senderUsername}.";
        }
    }
}
