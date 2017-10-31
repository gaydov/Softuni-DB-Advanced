namespace OldestFamilyMember.Models
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

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.name = value;
            }
        }

        public int Age  
        {
            get
            {
                return this.age;
            }

            private set
            {
                this.age = value;
            }
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Age}";
        }
    }
}