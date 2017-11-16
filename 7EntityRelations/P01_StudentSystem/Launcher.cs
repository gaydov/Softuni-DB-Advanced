using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Enums;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    public class Launcher
    {
        public static void Main()
        {
            StudentSystemContext context = new StudentSystemContext();

            using (context)
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                Seed(context);
            }
        }

        private static void Seed(StudentSystemContext context)
        {
            Course cSharpCourse = new Course
            {
                Description = "hardcore programming",
                Name = "C#",
                StartDate = DateTime.ParseExact("20-11-2017", "dd-MM-yyyy", null),
                EndDate = DateTime.ParseExact("20-12-2018", "dd-MM-yyyy", null),
                Price = 500.00m
            };

            context.Courses.Add(cSharpCourse);

            Course javaCourse = new Course
            {
                Description = "Oracle programming",
                Name = "Java",
                StartDate = DateTime.ParseExact("10-11-2017", "dd-MM-yyyy", null),
                EndDate = DateTime.ParseExact("10-12-2018", "dd-MM-yyyy", null),
                Price = 650.00m
            };

            context.Courses.Add(javaCourse);

            Student cSharpStudent = new Student
            {
                Name = "Ivan",
                PhoneNumber = "*88",
                RegisteredOn = DateTime.ParseExact("10-01-2017", "dd-MM-yyyy", null)
            };

            context.Students.Add(cSharpStudent);

            Student javaStudent = new Student
            {
                Name = "Georgi",
                PhoneNumber = "123",
                RegisteredOn = DateTime.ParseExact("18-05-2017", "dd-MM-yyyy", null)
            };

            context.Students.Add(javaStudent);

            Resource cSharpResource = new Resource
            {
                Course = cSharpCourse,
                Name = "C# Tasks",
                ResourceType = ResourceType.Document,
                Url = $"www.courses./{cSharpCourse.Name}"
            };

            context.Resources.Add(cSharpResource);

            Resource javaResource = new Resource
            {
                Course = cSharpCourse,
                Name = "Java tasks",
                ResourceType = ResourceType.Document,
                Url = $"www.courses./{javaCourse.Name}"
            };

            context.Resources.Add(javaResource);

            Homework cSharpHomework = new Homework
            {
                Student = cSharpStudent,
                Course = cSharpCourse,
                Content = "C# problems",
                ContentType = ContentType.Pdf,
                SubmissionTime = DateTime.ParseExact("20-12-2017", "dd-MM-yyyy", null)
            };

            context.HomeworkSubmissions.Add(cSharpHomework);

            Homework javaHomework = new Homework()
            {
                Student = javaStudent,
                Course = javaCourse,
                Content = "Java problems",
                ContentType = ContentType.Pdf,
                SubmissionTime = DateTime.ParseExact("20-01-2018", "dd-MM-yyyy", null)
            };

            context.HomeworkSubmissions.Add(javaHomework);

            StudentCourse cSharpAssignment = new StudentCourse
            {
                Student = cSharpStudent,
                Course = cSharpCourse
            };

            context.StudentCourses.Add(cSharpAssignment);

            StudentCourse javaAssignment = new StudentCourse
            {
                Student = cSharpStudent,
                Course = javaCourse
            };

            context.StudentCourses.Add(javaAssignment);

            context.SaveChanges();
        }
    }
}
