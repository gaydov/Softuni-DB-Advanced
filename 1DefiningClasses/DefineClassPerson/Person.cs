namespace DefineClassPerson
{
    public class Person
    {
        public Person()
            : this(string.Empty, 0)
        {
        }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
