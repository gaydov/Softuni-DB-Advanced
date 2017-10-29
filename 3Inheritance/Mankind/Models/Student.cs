using System;
using System.Linq;
using System.Text;
using Mankind.Utilities;

namespace Mankind.Models
{
    public class Student : Human
    {
        private string facNumber;

        public Student(string firstName, string lastName, string facultyNumber)
            : base(firstName, lastName)
        {
            this.FacNumber = facultyNumber;
        }

        public string FacNumber
        {
            get
            {
                return this.facNumber;
            }

            private set
            {
                if (value.Length < Constants.StudentFacNumberMinLenght || value.Length > Constants.StudentFacNumberMaxLenght
                    || !value.All(char.IsLetterOrDigit))
                {
                    throw new ArgumentException(ErrorMessages.StudentInvalidFacultyNumber);
                }

                this.facNumber = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Faculty number: {this.facNumber}");

            return sb.ToString();
        }
    }
}