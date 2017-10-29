using System;
using Person.Utilities;

namespace Person.Models
{
    public class Child : Person
    {
        public Child(string name, int age)
            : base(name, age)
        {
        }

        public override int Age
        {
            get
            {
                return base.Age;
            }

            protected set
            {
                if (value > 15)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.ChildInvalidAge, Constants.ChildMaxAge));
                }

                base.Age = value;
            }
        }
    }
}