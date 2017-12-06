using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Instagraph.Data;
using Instagraph.DataProcessor.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            UncommentedPostDto[] uncommentedPosts = context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Picture)
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .ProjectTo<UncommentedPostDto>()
                .ToArray();

            string serializedUncommentedPosts = JsonConvert.SerializeObject(uncommentedPosts, Formatting.Indented);
            return serializedUncommentedPosts;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            PopularUserDto[] popularUsers = context.Users
                .Include(u => u.Followers)
                .Include(u => u.UsersFollowing)
                .Include(u => u.Comments)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .Where(u => u.Posts.Any(p => p.Comments.Any(c => u.Followers.Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .ProjectTo<PopularUserDto>()
                .ToArray();

            string serializedPopularUsers = JsonConvert.SerializeObject(popularUsers, Formatting.Indented);
            return serializedPopularUsers;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            UserWithComments[] users = context.Users
                .Include(u => u.Comments)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .ProjectTo<UserWithComments>()
                .ToArray();

            List<UserWithMostComments> resultUsers = new List<UserWithMostComments>();

            foreach (UserWithComments userWithComments in users)
            {
                int mostCommentsCount = 0;

                if (userWithComments.PostsCommentsCount.Count > 0)
                {
                    mostCommentsCount = userWithComments.PostsCommentsCount.OrderByDescending(x => x).First();
                }

                UserWithMostComments currentUser = new UserWithMostComments
                {
                    Username = userWithComments.Username,
                    MostComments = mostCommentsCount
                };

                resultUsers.Add(currentUser);
            }

            resultUsers = resultUsers.OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToList();

            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", null));
            doc.Add(new XElement("users"));

            foreach (UserWithMostComments userWithMostComments in resultUsers)
            {
                XElement userElement = new XElement("user",
                                            new XElement("Username", userWithMostComments.Username),
                                            new XElement("MostComments", userWithMostComments.MostComments));

                doc.Root.Add(userElement);
            }

            return doc.ToString();
        }
    }
}
