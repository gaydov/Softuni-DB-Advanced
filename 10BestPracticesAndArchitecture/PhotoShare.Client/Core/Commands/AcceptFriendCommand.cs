using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class AcceptFriendCommand : Command
    {
        // AcceptFriend <username1> <username2>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string receiverUsername = data[0];
            string senderUsername = data[1];

            User receiver = context
                .Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username.Equals(receiverUsername));

            if (receiver == null)
            {
                throw new ArgumentException($"{receiverUsername} not found!");
            }

            User sender = context
                .Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username.Equals(senderUsername));

            if (sender == null)
            {
                throw new ArgumentException($"{senderUsername} not found!");
            }

            if (receiver.FriendsAdded.Any(f => f.Friend.Username.Equals(senderUsername)))
            {
                throw new InvalidOperationException($"{senderUsername} is already a friend to {receiverUsername}!");
            }

            if (!sender.FriendsAdded.Any(f => f.Friend.Username.Equals(receiverUsername)))
            {
                throw new InvalidOperationException($"{senderUsername} has not added {receiverUsername} as a friend!");
            }

            Friendship friendship = new Friendship
            {
                User = receiver,
                Friend = sender
            };

            receiver.FriendsAdded.Add(friendship);
            context.SaveChanges();

            return $"{receiverUsername} accepted {senderUsername} as a friend.";
        }
    }
}
