using System;
using System.Collections.Generic;
using System.Linq;

namespace OpinionPoll
{
    public class Launcher
    {
        public static void Main()
        {
            int peopleCount = int.Parse(Console.ReadLine());
            IList<Person> people = new List<Person>();

            for (int i = 0; i < peopleCount; i++)
            {
                string[] input = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string currentPersonName = input[0];
                int currentPersonAge = int.Parse(input[1]);

                Person currentPerson = new Person(currentPersonName, currentPersonAge);
                people.Add(currentPerson);
            }

            foreach (Person person in people.Where(p => p.Age > 30).OrderBy(p => p.Name))
            {
                Console.WriteLine(person);
            }
        }
    }
}