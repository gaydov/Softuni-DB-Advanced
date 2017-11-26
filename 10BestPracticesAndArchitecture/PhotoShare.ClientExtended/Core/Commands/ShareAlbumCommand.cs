using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class ShareAlbumCommand : Command
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public override string Execute(string[] data, PhotoShareContext context)
        {
            int albumId = int.Parse(data[0]);

            Album album = context.Albums
                .Include(a => a.AlbumRoles)
                .SingleOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            string username = data[1];

            User user = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            bool isPermissionValid = Enum.TryParse(data[2], true, out Role permissionRole);

            if (!isPermissionValid)
            {
                throw new ArgumentException("Permission must be either “Owner” or “Viewer”!");
            }

            AlbumRole role = new AlbumRole
            {
                Album = album,
                User = user,
                Role = permissionRole
            };

            if (album.AlbumRoles.Any(r => r.UserId == user.UserId && r.AlbumId == album.Id))
            {
                Role currentRole = album.AlbumRoles.Single(r => r.UserId == user.UserId && r.AlbumId == album.Id).Role;

                throw new ArgumentException($"User {username} has already assigned {currentRole.ToString()} role to album {album.Name}.");
            }

            if (!album.AlbumRoles.Select(r => r.UserId).Contains(Session.User.UserId)
                || album.AlbumRoles.Single(r => r.UserId == Session.User.UserId).Role != Role.Owner)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            album.AlbumRoles.Add(role);
            context.SaveChanges();

            return $"Username {user.Username} added to album {album.Name} ({permissionRole.ToString()})";
        }
    }
}
