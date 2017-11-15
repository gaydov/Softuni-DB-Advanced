using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddPrescriptionCommand : Command
    {
        public AddPrescriptionCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer) 
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            FindPatientByIdCommand findPatientByIdCommand = new FindPatientByIdCommand(this.Context, this.IsDoctorLogged, this.LoggedDoctorId, this.Reader, this.Writer);

            Patient patient = findPatientByIdCommand.TryFindPatientById();

            if (!patient.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
            {
                this.Writer.Write(Environment.NewLine);
                this.Writer.WriteLine(string.Format(ErrorMessages.PatientNotVisitedByThisDoctor, "prescription"));
                this.Writer.Write(Environment.NewLine);
                return;
            }

            Medicament medicament = this.TryFindMedicamentByName();
            PatientMedicament prescription = this.TryCreatePrescription(patient, medicament);

            this.Context.Prescriptions.Add(prescription);
            this.Context.SaveChanges();

            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine(string.Format(InfoMessages.SuccessfullyPrescribedMedication, medicament.Name, patient.FirstName, patient.LastName));
        }

        private Medicament TryFindMedicamentByName()
        {
            string medicamentName = Helpers.IsNullOrEmptyValidator("Medicament name: ");
            Medicament medicament = this.Context.Medicaments.FirstOrDefault(f => f.Name.Equals(medicamentName));

            if (medicament == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.MedicamentNotFound, medicamentName));
            }

            return medicament;
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