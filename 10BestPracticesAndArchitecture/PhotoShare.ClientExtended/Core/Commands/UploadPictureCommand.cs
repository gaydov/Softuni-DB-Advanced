using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public class UploadPictureCommand : Command
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string albumName = data[0];
            string picTitle = data[1];
            string picPath = data[2];

            Album album = context.Albums
                .Include(a => a.Pictures)
                .Include(a => a.AlbumRoles)
                .SingleOrDefault(a => a.Name.Equals(albumName));

            if (album == null)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            if (!album.AlbumRoles.Select(r => r.UserId).Contains(Session.User.UserId)
                || album.AlbumRoles.Single(r => r.UserId == Session.User.UserId).Role != Role.Owner)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            Picture currentPicture = new Picture
            {
                Title = picTitle,
                Path = picPath,
                Album = album
            };

            album.Pictures.Add(currentPicture);
            context.SaveChanges();

            return $"Picture {picTitle} added to {albumName}!";
        }
    }
}
