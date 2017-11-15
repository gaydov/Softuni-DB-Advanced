using System;
using System.Text;

namespace HospitalDbExtended.Data.Models
{
    public class Visitation
    {
        public Visitation()
        {
        }

        public Visitation(DateTime date, Patient patient, Doctor doctor, string comments)
        {
            this.Date = date;
            this.Patient = patient;
            this.Doctor = doctor;
            this.Comments = comments;
        }

        public int VisitationId { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"   {this.Date} - Patient: \"{this.Patient.FirstName} {this.Patient.LastName}\", Comments: ");

            if (string.IsNullOrWhiteSpace(this.Comments))
            {
                sb.Append("no comments.");
            }
            else
            {
                sb.Append($"{this.Comments}.");
            }

            return sb.ToString();
        }
    }
}