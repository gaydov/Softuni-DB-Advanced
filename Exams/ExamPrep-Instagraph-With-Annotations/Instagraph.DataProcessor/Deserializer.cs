using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Instagraph.Data;
using Instagraph.DataProcessor.DTOs.Import;
using Instagraph.Models;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        private const string SuccessfullyImportedEntityMsg = "Successfully imported {0} {1}.";
        private const string SuccessfullyImportedUserFollowerMsg = "Successfully imported Follower {0} to User {1}.";
        private const string FailureMsg = "Error: Invalid data.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            Picture[] picturesFromJson = ImportFromJson<Picture>(jsonString, false);
            StringBuilder sb = new StringBuilder();
            List<Picture> resultPictures = new List<Picture>();

            foreach (Picture picture in picturesFromJson)
            {
                if (!IsValid(picture))
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                if (resultPictures.Any(p => p.Path == picture.Path) || picture.Size <= 0)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                resultPictures.Add(picture);
                sb.AppendLine(string.Format(SuccessfullyImportedEntityMsg, nameof(Picture), picture.Path));
            }

            context.Pictures.AddRange(resultPictures);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            UserDto[] usersFromJson = ImportFromJson<UserDto>(jsonString, false);
            StringBuilder sb = new StringBuilder();
            List<User> resultUsers = new List<User>();

            foreach (UserDto userDto in usersFromJson)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                bool userAlreadyExists = context.Users.Any(u => u.Username == userDto.Username);
                bool pictureExists = context.Pictures.Any(p => p.Path == userDto.ProfilePicture);

                if (userAlreadyExists || !pictureExists)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                User currentUser = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = context.Pictures.Single(p => p.Path == userDto.ProfilePicture)
                };

                resultUsers.Add(currentUser);
                sb.AppendLine(string.Format(SuccessfullyImportedEntityMsg, nameof(User), currentUser.Username));
            }

            context.Users.AddRange(resultUsers);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            UserFollowerDto[] usersFollowersFromJson = ImportFromJson<UserFollowerDto>(jsonString, false);
            StringBuilder sb = new StringBuilder();
            List<UserFollower> resultUsersFollowers = new List<UserFollower>();

            foreach (UserFollowerDto userFollowerDto in usersFollowersFromJson)
            {
                if (!IsValid(userFollowerDto))
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                User user = context.Users.SingleOrDefault(u => u.Username == userFollowerDto.User);
                User follower = context.Users.SingleOrDefault(f => f.Username == userFollowerDto.Follower);

                if (user == null || follower == null)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                bool userFollowerAlreadyExists = resultUsersFollowers.Any(uf =>
                    uf.User.Username == userFollowerDto.User &&
                    uf.Follower.Username == userFollowerDto.Follower);

                if (userFollowerAlreadyExists)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                UserFollower currentUserFollower = new UserFollower
                {
                    User = user,
                    Follower = follower
                };

                resultUsersFollowers.Add(currentUserFollower);
                sb.AppendLine(string.Format(SuccessfullyImportedUserFollowerMsg, currentUserFollower.Follower.Username,
                    currentUserFollower.User.Username));
            }

            context.UsersFollowers.AddRange(resultUsersFollowers);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PostDto[]), new XmlRootAttribute("posts"));
            PostDto[] postsFromXml = (PostDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Post> resultPosts = new List<Post>();

            foreach (PostDto postDto in postsFromXml)
            {
                if (!IsValid(postDto))
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                User user = context.Users.SingleOrDefault(u => u.Username == postDto.User);
                Picture picture = context.Pictures.SingleOrDefault(p => p.Path == postDto.Picture);

                if (user == null || picture == null)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                Post currentPost = new Post
                {
                    Caption = postDto.Caption,
                    User = user,
                    Picture = picture
                };

                resultPosts.Add(currentPost);
                sb.AppendLine(string.Format(SuccessfullyImportedEntityMsg, nameof(Post), currentPost.Caption));
            }

            context.Posts.AddRange(resultPosts);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CommentDto[]), new XmlRootAttribute("comments"));
            CommentDto[] commentsFromXml = (CommentDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Comment> resultComments = new List<Comment>();

            foreach (CommentDto commentDto in commentsFromXml)
            {
                if (!IsValid(commentDto))
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                User user = context.Users.SingleOrDefault(u => u.Username == commentDto.User);
                Post post = context.Posts.SingleOrDefault(p => p.Id == commentDto.Post.Id);

                if (user == null || post == null)
                {
                    sb.AppendLine(FailureMsg);
                    continue;
                }

                Comment currentComment = new Comment
                {
                    Content = commentDto.Content,
                    User = user,
                    Post = post
                };

                resultComments.Add(currentComment);
                sb.AppendLine(string.Format(SuccessfullyImportedEntityMsg, nameof(Comment), currentComment.Content));
            }

            context.Comments.AddRange(resultComments);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        private static T[] ImportFromJson<T>(string jsonString, bool ignoreNullValues)
        {
            T[] deserializedObjects;

            if (ignoreNullValues)
            {
                deserializedObjects = JsonConvert.DeserializeObject<T[]>(jsonString, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            else
            {
                deserializedObjects = JsonConvert.DeserializeObject<T[]>(jsonString);
            }

            return deserializedObjects;
        }

        private static bool IsValid(object obj)
        {
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}
