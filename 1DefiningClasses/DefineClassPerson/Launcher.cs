using System;
using System.Reflection;

namespace DefineClassPerson
{
    public class Launcher
    {
        public static void Main()
        {
            Person firstPerson = new Person("Pesho", 20);
            Person secondPerson = new Person("Gosho", 18);
            Person thirdPerson = new Person
            {
                Name = "Stamat",
                Age = 43
            };

            Type personType = typeof(Person);
            PropertyInfo[] properties = personType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine(properties.Length);
        }
    }
}