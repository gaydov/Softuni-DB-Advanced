namespace HospitalDbExtended.Data.Models
{
    public class PatientMedicament 
    {
        public PatientMedicament()
        {
        }

        public PatientMedicament(Patient patient, Medicament medicament)
        {
            this.Patient = patient;
            this.Medicament = medicament;
        }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int MedicamentId { get; set; }

        public Medicament Medicament { get; set; }

        public override string ToString()
        {
            return $"   Patient \"{this.Patient.FirstName} {this.Patient.LastName}\" - \"{this.Medicament.Name}\"";
        }
    }
}