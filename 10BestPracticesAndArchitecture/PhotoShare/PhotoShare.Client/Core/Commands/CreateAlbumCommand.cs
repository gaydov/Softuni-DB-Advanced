using System;
using System.Linq;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class CreateAlbumCommand : Command
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string username = data[0];
            string title = data[1];
            bool isColorValid = Enum.TryParse(data[2], true, out Color color);
            string[] tags = data.Skip(3).Select(t => t.ValidateOrTransform()).ToArray();

            User currentUser = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (context.Albums.Any(a => a.Name.Equals(title)))
            {
                throw new ArgumentException($"Album {title} exists!");
            }

            if (!isColorValid)
            {
                throw new ArgumentException($"Color {data[2]} not found!");
            }

            if (tags.Except(context.Tags.Select(t => t.Name)).Any())
            {
                throw new ArgumentException("Invalid tags!");
            }

            Album album = new Album
            {
                BackgroundColor = color,
                Name = title,
                IsPublic = true
            };

            AlbumRole albumRole = new AlbumRole
            {
                User = currentUser,
                Album = album,
                Role = Role.Owner
            };

            album.AlbumRoles.Add(albumRole);
            context.Albums.Add(album);
            context.SaveChanges();

            foreach (string tag in tags)
            {
                Tag currentTag = context.Tags.SingleOrDefault(t => t.Name.Equals(tag));
                Album currentAlbum = context.Albums.SingleOrDefault(a => a.Name.Equals(title));

                AlbumTag albumTag = new AlbumTag
                {
                    Tag = currentTag,
                    Album = currentAlbum
                };

                currentAlbum.AlbumTags.Add(albumTag);
            }

            context.SaveChanges();

            return $"Album {album.Name} successfully created!";
        }
    }
}