namespace HospitalDbExtended.Data.Models
{
    public class Diagnose
    {
        public Diagnose()
        {
        }

        public Diagnose(string name, string comments, Patient patient)
        {
            this.Name = name;
            this.Comments = comments;
            this.Patient = patient;
        }

        public int DiagnoseId { get; set; }

        public string Name { get; set; }

        public string Comments { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public override string ToString()
        {
            return $"   {this.Name} - {this.Comments} for patient \"{this.Patient.FirstName} {this.Patient.LastName}\".";
        }
    }
}