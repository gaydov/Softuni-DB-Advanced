using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            this.CourseEnrollments = new HashSet<StudentCourse>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }

        public Student(string name, string phoneNumber, DateTime registeredDate)
            : this()
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.RegisteredOn = registeredDate;
        }

        public int StudentId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> CourseEnrollments { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{nameof(this.Name)}: {this.Name}");
            sb.AppendLine($"{nameof(this.PhoneNumber)}: {this.PhoneNumber}");
            sb.AppendLine($"Registration date: {this.RegisteredOn}");

            return sb.ToString();
        }
    }
}