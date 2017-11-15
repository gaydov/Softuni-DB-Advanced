using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddVisitationCommand : Command
    {
        public AddVisitationCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            this.AddVisitation(patientId);
        }

        public void AddVisitation(int patientId)
        {
            DateTime date = Helpers.TryParseDateInCertainFormat("dd/MM/yyyy HH:mm");

            this.Writer.Write("Comments: ");
            string comments = this.Reader.ReadLine();

            Patient patient = this.Context.Patients.FirstOrDefault(p => p.PatientId == patientId);
            Doctor doctor = this.Context.Doctors.FirstOrDefault(d => d.DoctorId == this.LoggedDoctorId);
            Visitation visitation = new Visitation(date, patient, doctor, comments);
            this.Context.Visitations.Add(visitation);
            this.Context.SaveChanges();

            this.Writer.WriteLine(string.Format(InfoMessages.SuccessfullyAddedVisitation, visitation.Date, visitation.Patient.FirstName, visitation.Patient.LastName));
        }
    }
}