using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalDbExtended.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            this.Visitations = new List<Visitation>();
            this.Diagnoses = new List<Diagnose>();
            this.Prescriptions = new List<PatientMedicament>();
        }

        public Patient(string firstName, string lastName, string address, string email, bool hasInsurance)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.Email = email;
            this.HasInsurance = hasInsurance;
            this.Visitations = new List<Visitation>();
            this.Diagnoses = new List<Diagnose>();
            this.Prescriptions = new List<PatientMedicament>();
        }

        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            string hasInsuranceStr = "No";
            if (this.HasInsurance)
            {
                hasInsuranceStr = "Yes";
            }

            sb.AppendLine($"   ID: {this.PatientId}, Names: \"{this.FirstName} {this.LastName}\", Address: \"{this.Address}\", Email: {this.Email}, Insured: {hasInsuranceStr}");
            sb.Append(Environment.NewLine);

            sb.AppendLine("    Visitations:");
            foreach (Visitation visitation in this.Visitations)
            {
                sb.AppendLine($"    {visitation}");
            }

            sb.Append(Environment.NewLine);

            sb.AppendLine("    Diagnoses:");
            foreach (Diagnose diagnosis in this.Diagnoses)
            {
                sb.AppendLine($"    {diagnosis}");
            }

            sb.Append(Environment.NewLine);

            sb.AppendLine("    Prescriptions:");
            foreach (PatientMedicament prescription in this.Prescriptions)
            {
                sb.Append($"    {prescription}");
            }

            return sb.ToString();
        }
    }
}