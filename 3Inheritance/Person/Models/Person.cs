using System;
using Person.Utilities;

namespace Person.Models
{
    public class Person
    {
        private string name;
        private int age;

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }

            protected set
            {
                if (value.Length < 3)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.NameInvalidLenght, Constants.PersonNameMinLenght));
                }

                this.name = value;
            }
        }

        public virtual int Age
        {
            get
            {
                return this.age;
            }

            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ErrorMessages.PersonAgeNegative);
                }

                this.age = value;
            }
        }

        public override string ToString()
        {
            return $"Name: {this.Name}, Age: {this.Age}";
        }
    }
}