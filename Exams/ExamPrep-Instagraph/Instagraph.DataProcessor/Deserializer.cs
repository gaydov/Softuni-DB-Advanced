using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Instagraph.Data;
using Instagraph.DataProcessor.DTOs;
using Instagraph.Models;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        private const string ImportedEntitySuccessMsg = "Successfully imported {0} {1}.";
        private const string ImportedUserFollowerSuccessMsg = "Successfully imported Follower {0} to User {1}.";
        private const string ErrorMsg = "Error: Invalid data.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            List<Picture> picsFromJson = ImportFromJson<Picture>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Picture> resultPics = new List<Picture>();

            foreach (Picture picture in picsFromJson)
            {
                bool isPicValid = !string.IsNullOrWhiteSpace(picture.Path) && picture.Size > 0;
                bool picAlreadyExists = resultPics.Any(p => p.Path == picture.Path);

                if (!isPicValid || picAlreadyExists)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                resultPics.Add(picture);
                sb.AppendLine(string.Format(ImportedEntitySuccessMsg, nameof(Picture), picture.Path));
            }

            context.Pictures.AddRange(resultPics);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            List<UserDto> usersFromJson = ImportFromJson<UserDto>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<User> resultUsers = new List<User>();

            foreach (UserDto userDto in usersFromJson)
            {
                bool isUserValid = !string.IsNullOrWhiteSpace(userDto.Username) &&
                                   !string.IsNullOrWhiteSpace(userDto.Password) &&
                                   !string.IsNullOrWhiteSpace(userDto.ProfilePicture) &&
                                   userDto.Username.Length <= 30 &&
                                   userDto.Password.Length <= 20 &&
                                   context.Pictures.Any(p => p.Path == userDto.ProfilePicture);

                bool userAlreadyExists = resultUsers.Any(u => u.Username == userDto.Username);

                if (!isUserValid || userAlreadyExists)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                Picture currentUserPicture = context.Pictures.Single(p => p.Path == userDto.ProfilePicture);
                User currentUser = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = currentUserPicture
                };

                resultUsers.Add(currentUser);
                sb.AppendLine(string.Format(ImportedEntitySuccessMsg, nameof(User), currentUser.Username));
            }

            context.Users.AddRange(resultUsers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            List<UserFollowerDto> usersFollowersFromJson = ImportFromJson<UserFollowerDto>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<UserFollower> resultUsersFollowers = new List<UserFollower>();

            foreach (UserFollowerDto userFollowerDto in usersFollowersFromJson)
            {
                bool isUserFollowerValid = !string.IsNullOrWhiteSpace(userFollowerDto.User) &&
                                           !string.IsNullOrWhiteSpace(userFollowerDto.Follower);

                bool bothUsersExist = context.Users.Any(u => u.Username == userFollowerDto.User) &&
                                      context.Users.Any(u => u.Username == userFollowerDto.Follower);

                bool alreadyFollowed = resultUsersFollowers.Any(uf => uf.User.Username == userFollowerDto.User &&
                                                                      uf.Follower.Username == userFollowerDto.Follower);

                if (!isUserFollowerValid || !bothUsersExist || alreadyFollowed)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                User currentUser = context.Users.Single(u => u.Username == userFollowerDto.User);
                User currentFollower = context.Users.Single(u => u.Username == userFollowerDto.Follower);
                UserFollower currentUserFollower = new UserFollower
                {
                    User = currentUser,
                    Follower = currentFollower
                };

                resultUsersFollowers.Add(currentUserFollower);
                sb.AppendLine(string.Format(ImportedUserFollowerSuccessMsg, currentUserFollower.Follower.Username, currentUserFollower.User.Username));
            }

            context.UsersFollowers.AddRange(resultUsersFollowers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            XElement[] postsFromXml = doc.Root?.Elements().ToArray();
            StringBuilder sb = new StringBuilder();
            List<Post> resultPosts = new List<Post>();

            foreach (XElement xmlPost in postsFromXml)
            {
                string caption = xmlPost.Element("caption")?.Value;
                string userName = xmlPost.Element("user")?.Value;
                string picturePath = xmlPost.Element("picture")?.Value;

                bool isPostValid = !string.IsNullOrWhiteSpace(caption) &&
                                   !string.IsNullOrWhiteSpace(userName) &&
                                   !string.IsNullOrWhiteSpace(picturePath) &&
                                   context.Users.Any(u => u.Username == userName) &&
                                   context.Pictures.Any(p => p.Path == picturePath);

                if (!isPostValid)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                Picture currentPostPicture = context.Pictures.Single(p => p.Path == picturePath);
                User currentPostUser = context.Users.Single(u => u.Username == userName);
                Post currentPost = new Post
                {
                    Caption = caption,
                    Picture = currentPostPicture,
                    User = currentPostUser
                };

                resultPosts.Add(currentPost);
                sb.AppendLine(string.Format(ImportedEntitySuccessMsg, nameof(Post), currentPost.Caption));
            }

            context.Posts.AddRange(resultPosts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            XElement[] commentsFromXml = doc.Root?.Elements().ToArray();
            StringBuilder sb = new StringBuilder();
            List<Comment> resultComments = new List<Comment>();

            foreach (XElement commentXml in commentsFromXml)
            {
                string content = commentXml.Element("content")?.Value;
                string userName = commentXml.Element("user")?.Value;
                string postIdString = commentXml.Element("post")?.Attribute("id")?.Value;

                if (postIdString == null)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                int postId = int.Parse(postIdString);

                bool isCommentValid = !string.IsNullOrWhiteSpace(content) &&
                                      !string.IsNullOrWhiteSpace(userName) &&
                                      !string.IsNullOrWhiteSpace(postIdString) &&
                                      context.Users.Any(u => u.Username == userName) &&
                                      context.Posts.Any(p => p.Id == postId);

                if (!isCommentValid)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                User currentCommentUser = context.Users.Single(u => u.Username == userName);
                Post currentCommentPost = context.Posts.Single(p => p.Id == postId);
                Comment currentComment = new Comment
                {
                    Content = content,
                    User = currentCommentUser,
                    Post = currentCommentPost
                };

                resultComments.Add(currentComment);
                sb.AppendLine(string.Format(ImportedEntitySuccessMsg, nameof(Comment), currentComment.Content));
            }

            context.Comments.AddRange(resultComments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static List<T> ImportFromJson<T>(string jsonString)
        {
            List<T> deserializedObjects = JsonConvert.DeserializeObject<List<T>>(jsonString);

            return deserializedObjects;
        }
    }
}
