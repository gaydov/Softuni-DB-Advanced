using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Instagraph.Data;
using Instagraph.DataProcessor.DTOs.Export;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            UncommentedPostDto[] uncommentedPosts = context.Posts
                .Include(p => p.Comments)
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .Select(p => new UncommentedPostDto
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .ToArray();

            string serializedUncommentedPosts = JsonConvert.SerializeObject(uncommentedPosts, Formatting.Indented);
            return serializedUncommentedPosts;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            PopularUserDto[] popularUsers = context.Users
                .Include(u => u.Followers)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .Where(u => u.Posts.Any(p => p.Comments.Any(c => u.Followers.Any(f => f.FollowerId == c.UserId))))
                .Select(u => new PopularUserDto
                {
                    Username = u.Username,
                    Followers = u.Followers.Count
                })
                .ToArray();

            string serializedPopularUsers = JsonConvert.SerializeObject(popularUsers, Formatting.Indented);
            return serializedPopularUsers;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            UserWithMostCommentsDto[] users = context.Users
                 .Include(u => u.Posts)
                 .ThenInclude(p => p.Comments)
                 .Include(u => u.Comments)
                 .Select(u => new UserWithMostCommentsDto
                 {
                     Username = u.Username,
                     MostComments = u.Posts.Any() ? u.Posts.Max(p => p.Comments.Count) : 0
                 })
                 .OrderByDescending(u => u.MostComments)
                 .ThenBy(u => u.Username)
                 .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(UserWithMostCommentsDto[]), new XmlRootAttribute("users"));
            serializer.Serialize(new StringWriter(sb), users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            string serializedUsers = sb.ToString();
            return serializedUsers;
        }
    }
}
