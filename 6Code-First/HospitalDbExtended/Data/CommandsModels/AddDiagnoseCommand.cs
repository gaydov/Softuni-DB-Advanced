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
        public AddDiagnoseCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            this.AddDiagnose(patientId);
        }

        public void AddDiagnose(int patientId)
        {
            Patient patient = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
            {
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientWithIdNotFound, patientId));
                return;
            }

            if (!patient.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
            {
                this.Writer.Write(Environment.NewLine);
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientNotVisitedByThisDoctor, "diagnose"));
                this.Writer.Write(Environment.NewLine);
                return;
            }

            string diagnoseName = Helpers.IsNullOrEmptyValidator("Diagnose name: ");
            string diagnoseComments = Helpers.IsNullOrEmptyValidator("Diagnose comments: ");

            Diagnose diagnose = new Diagnose(diagnoseName, diagnoseComments, patient);
            this.Context.Diagnoses.Add(diagnose);
            this.Context.SaveChanges();

            this.Writer.WriteLine(string.Format(InfoMessages.SuccessfullyAddedDiagnose, diagnose.Name, patient.FirstName, patient.LastName));
        }
    }
}