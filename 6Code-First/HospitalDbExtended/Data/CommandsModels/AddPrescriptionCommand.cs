using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddPrescriptionCommand : Command
    {
        public AddPrescriptionCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer) 
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            FindPatientByIdCommand findPatientByIdCommand = new FindPatientByIdCommand(this.Context, this.IsLogged, this.LoggedDoctorId, this.Reader, this.Writer);

            Patient currentPatient = findPatientByIdCommand.TryFindPatientById();

            if (!currentPatient.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
            {
                this.Writer.Write(Environment.NewLine);
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientDoesNotHaveVisitations, "prescription"));
                this.Writer.Write(Environment.NewLine);
                return;
            }

            Medicament currentMedicament = this.TryFindMedicamentByName();

            PatientMedicament prescription = this.TryCreatePrescription(currentPatient, currentMedicament);

            this.Context.Prescriptions.Add(prescription);
            this.Context.SaveChanges();

            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine(string.Format(InfoMessages.SuccessfullyPrescribedMedication, currentMedicament.Name, currentPatient.FirstName, currentPatient.LastName));
            this.Writer.Write(Environment.NewLine);
        }

        private Medicament TryFindMedicamentByName()
        {
            string medicamentName = Helpers.IsNullOrEmptyValidator("Medicament name: ");
            Medicament currentMedicament = this.Context.Medicaments.FirstOrDefault(f => f.Name.Equals(medicamentName));

            if (currentMedicament == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.MedicamentNotFound, medicamentName));
            }

            return currentMedicament;
        }

        private PatientMedicament TryCreatePrescription(Patient patient, Medicament medicament)
        {
            if (this.Context.Prescriptions.Any(p => p.Patient.PatientId == patient.PatientId && p.Medicament.MedicamentId == medicament.MedicamentId))
            {
                throw new InvalidOperationException(string.Format(ErrorMessages.PrescriptionAlreadyExists, medicament.Name, patient.FirstName, patient.LastName));
            }

            PatientMedicament prescription = new PatientMedicament(patient, medicament);
            return prescription;
        }
    }
}