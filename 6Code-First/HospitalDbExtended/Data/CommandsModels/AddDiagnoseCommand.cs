using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddDiagnoseCommand : Command
    {
        public AddDiagnoseCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            this.AddDiagnose(patientId);
        }

        public void AddDiagnose(int patientId)
        {
            Patient currentPatient = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .FirstOrDefault(p => p.PatientId == patientId);

            if (currentPatient == null)
            {
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientWithIdNotFound, patientId));
                return;
            }

            if (!currentPatient.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
            {
                this.Writer.Write(Environment.NewLine);
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientDoesNotHaveVisitations, "diagnose"));
                this.Writer.Write(Environment.NewLine);
                return;
            }

            string diagnoseName = Helpers.IsNullOrEmptyValidator("Diagnose name: ");
            string diagnoseComments = Helpers.IsNullOrEmptyValidator("Diagnose comments: ");

            Diagnose currentDiagnose = new Diagnose(diagnoseName, diagnoseComments, currentPatient);
            this.Context.Diagnoses.Add(currentDiagnose);
            this.Context.SaveChanges();

            this.Writer.WriteLine(string.Format(InfoMessages.SuccessFullyAddedDiagnose, currentDiagnose.Name, currentPatient.FirstName, currentPatient.LastName));
        }
    }
}