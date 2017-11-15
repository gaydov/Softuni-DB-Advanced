using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class FindPatientByNameCommand : Command
    {
        public FindPatientByNameCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Patient[] searchedPatients = this.TryFindPatientsByName();
            this.Writer.Write(Environment.NewLine);

            foreach (Patient patient in searchedPatients)
            {
                this.Writer.WriteLine(patient.ToString());
            }
        }

        private Patient[] TryFindPatientsByName()
        {
            string firstName = Helpers.IsNullOrEmptyValidator("First name: ");
            string lastName = Helpers.IsNullOrEmptyValidator("Last name: ");

            Patient[] patients = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Medicament)
                .Where(p => p.FirstName.ToLower() == firstName.ToLower() && p.LastName.ToLower() == lastName.ToLower() && p.Visitations.Any(v => v.Doctor.DoctorId == this.LoggedDoctorId))
                .ToArray();

            if (patients.Length == 0)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PatientsWithNameNotFound, $"\"{firstName} {lastName}\""));
            }

            return patients;
        }
    }
}