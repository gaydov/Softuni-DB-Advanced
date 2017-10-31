using System;
using OldestFamilyMember.Models;

namespace OldestFamilyMember
{
    public class Launcher
    {
        public static void Main()
        {
            int peopleCount = int.Parse(Console.ReadLine());
            Family family = new Family();

            for (int i = 0; i < peopleCount; i++)
            {
                string[] personInfo = Console.ReadLine().Split();
                string personName = personInfo[0];
                int personAge = int.Parse(personInfo[1]);

                Person person = new Person(personName, personAge);
                family.AddMember(person);
            }

            Console.WriteLine(family.GetOldestMember());
        }
    }
}