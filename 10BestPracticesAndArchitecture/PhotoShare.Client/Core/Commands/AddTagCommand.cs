using System;
using System.Linq;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class AddTagCommand : Command
    {
        // AddTag <tag>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            string tagText = data[0].ValidateOrTransform();

            if (context.Tags.Any(t => t.Name.Equals(tagText)))
            {
                throw new ArgumentException($"Tag {tagText} exists!");
            }

            Tag currentTag = new Tag
            {
                Name = tagText
            };

            context.Tags.Add(currentTag);
            context.SaveChanges();

            return $"Tag {currentTag.Name} was added successfully!";
        }
    }
}
