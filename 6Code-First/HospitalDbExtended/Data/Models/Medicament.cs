using System.Collections.Generic;

namespace HospitalDbExtended.Data.Models
{
    public class Medicament
    {
        public Medicament()
        {
            this.Prescriptions = new List<PatientMedicament>();
        }

        public Medicament(string name)
        {
            this.Name = name;
        }

        public int MedicamentId { get; set; }

        public string Name { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

        public override string ToString()
        {
            return $"\"{this.Name}\"";
        }
    }
}