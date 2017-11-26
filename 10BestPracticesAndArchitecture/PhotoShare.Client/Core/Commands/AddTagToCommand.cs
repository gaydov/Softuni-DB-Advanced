using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class AddTagToCommand : Command
    {
        // AddTagTo <albumName> <tag>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string albumName = data[0];
            string tagText = data[1].ValidateOrTransform();

            if (!context.Tags.Any(t => t.Name.Equals(tagText, StringComparison.InvariantCultureIgnoreCase))
                || !context.Albums.Any(a => a.Name.Equals(albumName)))
            {
                throw new ArgumentException("Either tag or album does not exist!");
            }

            Tag currentTag = context.Tags.SingleOrDefault(t => t.Name.Equals(tagText));
            Album currentAlbum = context.Albums.SingleOrDefault(a => a.Name.Equals(albumName));

            AlbumTag currentAlbumTag = new AlbumTag
            {
                Album = currentAlbum,
                Tag = currentTag
            };

            currentAlbum.AlbumTags.Add(currentAlbumTag);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new ArgumentException($"Tag {tagText} has already been added to \"{albumName}\"!");
            }

            return $"Tag {currentTag.Name} added to {currentAlbum.Name}!";
        }
    }
}