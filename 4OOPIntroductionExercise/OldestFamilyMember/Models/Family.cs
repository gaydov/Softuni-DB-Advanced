using System.Collections.Generic;
using System.Linq;

namespace OldestFamilyMember.Models
{
    public class Family
    {
        private readonly IList<Person> people;

        public Family()
        {
            this.people = new List<Person>();
        }

        public void AddMember(Person member)
        {
            this.people.Add(member);
        }

        public Person GetOldestMember()
        {
            Person oldestPerson = this.people.OrderByDescending(p => p.Age).FirstOrDefault();
            return oldestPerson;
        }
    }
}