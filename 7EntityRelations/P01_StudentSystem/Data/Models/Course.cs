using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            this.StudentsEnrolled = new HashSet<StudentCourse>();
            this.Resources = new HashSet<Resource>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }

        public Course(string name, string description, DateTime startDate, DateTime endDate, decimal price)
            : this()
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Price = price;
        }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsEnrolled { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{nameof(this.Name)}: {this.Name}");
            sb.AppendLine($"{nameof(this.Description)}: {this.Description}");
            sb.AppendLine($"Start date: {this.StartDate}");
            sb.AppendLine($"End date: {this.EndDate}");
            sb.AppendLine($"{nameof(this.Price)}: {this.Price:F2}");

            return sb.ToString();
        }
    }
}