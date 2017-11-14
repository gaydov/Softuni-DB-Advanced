using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class FindPatientByIdCommand : Command
    {
        public FindPatientByIdCommand(HospitalContext context, bool isUserLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isUserLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Patient searchedPatient = this.TryFindPatientById();
            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine(searchedPatient.ToString());
        }

        public Patient TryFindPatientById()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            Patient patient = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Medicament)
                .FirstOrDefault(p => p.PatientId == patientId && p.Visitations.Any(v => v.Doctor.DoctorId == this.LoggedDoctorId));

            if (patient == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PatientWithIdNotFound, patientId));
            }

            return patient;
        }
    }
}