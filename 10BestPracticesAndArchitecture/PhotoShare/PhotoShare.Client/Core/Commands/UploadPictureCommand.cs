using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
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
                .SingleOrDefault(a => a.Name.Equals(albumName));

            if (album == null)
            {
                throw new ArgumentException($"Album {albumName} not found!");
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
