using System.Collections.Generic;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.Models
{
    public class Doctor
    {
        public Doctor()
        {
        }

        public Doctor(string name, string specialty, string username, string password)
        {
            this.Name = name;
            this.Specialty = specialty;
            this.Username = username;
            this.Salt = PasswordHasher.GenerateSalt();
            this.Password = PasswordHasher.GenerateHash(password + this.Salt);
            this.Visitations = new List<Visitation>();
        }

        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Specialty { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        internal string Password { get; set; }

        internal string Salt { get; set; }
    }
}