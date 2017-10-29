using System;
using System.Text;
using Mankind.Utilities;

namespace Mankind.Models
{
    public class Human
    {
        private string firstName;
        private string lastName;

        public Human(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            protected set
            {
                if (value.Length < Constants.FirstNameMinLenght)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.NameMinLenghtInvalid, Constants.FirstNameMinLenght, nameof(this.firstName)));
                }

                if (!char.IsUpper(value[0]))
                {
                    throw new ArgumentException(string.Format(ErrorMessages.NameInvalid, nameof(this.firstName)));
                }

                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            protected set
            {
                if (value.Length < Constants.LastNameMinLenght)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.NameMinLenghtInvalid, Constants.LastNameMinLenght, nameof(this.lastName)));
                }

                if (!char.IsUpper(value[0]))
                {
                    throw new ArgumentException(string.Format(ErrorMessages.NameInvalid, nameof(this.lastName)));
                }

                this.lastName = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"First Name: {this.FirstName}");
            sb.AppendLine($"Last Name: {this.LastName}");

            return sb.ToString().Trim();
        }
    }
}