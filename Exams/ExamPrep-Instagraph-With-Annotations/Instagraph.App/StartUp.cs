using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using AutoMapper;
using Instagraph.Data;
using Instagraph.DataProcessor;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(options => options.AddProfile<InstagraphProfile>());

            InstagraphContext context = new InstagraphContext();

            using (context)
            {
                ResetDatabase(context, true);
            }

            Console.WriteLine(ImportData());

            ExportData();
        }

        private static string ImportData()
        {
            StringBuilder sb = new StringBuilder();

            using (InstagraphContext context = new InstagraphContext())
            {
                string picturesJson = File.ReadAllText("../files/input/pictures.json");

                sb.AppendLine(Deserializer.ImportPictures(context, picturesJson));

                string usersJson = File.ReadAllText("../files/input/users.json");

                sb.AppendLine(Deserializer.ImportUsers(context, usersJson));
                
                string followersJson = File.ReadAllText("../files/input/users_followers.json");

                sb.AppendLine(Deserializer.ImportFollowers(context, followersJson));

                string postsXml = File.ReadAllText("../files/input/posts.xml");

                sb.AppendLine(Deserializer.ImportPosts(context, postsXml));

                string commentsXml = File.ReadAllText("../files/input/comments.xml");

                sb.AppendLine(Deserializer.ImportComments(context, commentsXml));
            }

            string result = sb.ToString().Trim();
            return result;
        }

        private static void ExportData()
        {
            using (InstagraphContext context = new InstagraphContext())
            {
                string uncommentedPostsOutput = Serializer.ExportUncommentedPosts(context);

                File.WriteAllText("../files/output/UncommentedPosts.json", uncommentedPostsOutput);

                string usersOutput = Serializer.ExportPopularUsers(context);

                File.WriteAllText("../files/output/PopularUsers.json", usersOutput);

                string commentsOutput = Serializer.ExportCommentsOnPosts(context);

                File.WriteAllText("../files/output/CommentsOnPosts.xml", commentsOutput);
            }
        }

        private static void ResetDatabase(InstagraphContext context, bool shouldDeleteDatabase)
        {
            if (shouldDeleteDatabase)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            context.Database.EnsureCreated();

            string disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            string deleteRowsQuery = "EXEC sp_MSforeachtable @command1='DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            string enableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            string reseedQuery = "EXEC sp_MSforeachtable @command1='DBCC CHECKIDENT(''?'', RESEED, 0)'";
            try
            {
                context.Database.ExecuteSqlCommand(reseedQuery);
            }
            catch (SqlException)
            {
                // OrderItems table has no identity column, which isn't a problem
            }
        }
    }
}
