using System;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddPatientCommand : Command
    {
        public AddPatientCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Patient patient = this.ReadPatientInfoAndCreatePatient();
            this.Context.Patients.Add(patient);
            this.Context.SaveChanges();

            bool shouldAddVisitations = true;

            while (shouldAddVisitations)
            {
                this.Writer.WriteLine("Visitation information:");
                AddVisitationCommand addVisitationCommand = new AddVisitationCommand(this.Context, this.IsDoctorLogged, this.LoggedDoctorId, this.Reader, this.Writer);
                addVisitationCommand.AddVisitation(patient.PatientId);
                this.Writer.Write(Environment.NewLine);
                shouldAddVisitations = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldMoreCollectionEntitiesBeAdded, "visitations"));
            }

            bool shouldAddDiagnoses = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldCollectionEntitiesBeAdded, "diagnoses"));

            while (shouldAddDiagnoses)
            {
                AddDiagnoseCommand addDiagnoseCommand = new AddDiagnoseCommand(this.Context, this.IsDoctorLogged, this.LoggedDoctorId, this.Reader, this.Writer);
                addDiagnoseCommand.AddDiagnose(patient.PatientId);
                this.Writer.Write(Environment.NewLine);
                shouldAddDiagnoses = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldMoreCollectionEntitiesBeAdded, "diagnoses"));
            }

            this.Writer.WriteLine(string.Format(InfoMessages.SuccessfullyAddedPatient));
            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine(patient.ToString());
        }

        private Patient ReadPatientInfoAndCreatePatient()
        {
            string firstName = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} first name: ");
            string lastName = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} last name: ");
            this.Writer.Write($"{nameof(Patient)} address: ");
            string address = this.Reader.ReadLine();
            string email = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} email: ");
            bool hasInsurance = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.PatientHasInsurance, nameof(Patient).ToLower()));

            Patient patient = new Patient(firstName, lastName, address, email, hasInsurance);
            return patient;
        }
    }
}