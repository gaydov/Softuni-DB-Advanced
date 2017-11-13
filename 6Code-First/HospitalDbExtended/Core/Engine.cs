using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HospitalDbExtended.Core.IO;
using HospitalDbExtended.Data;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace HospitalDbExtended.Core
{
    public class Engine
    {
        private readonly HospitalContext context;
        private bool isLogged;
        private int loggedDoctorId;

        public Engine(HospitalContext context)
        {
            this.context = context;
            this.isLogged = false;
        }

        public void Run()
        {
            this.RegisterOrLogin();
            this.InterpretCommand();
        }

        public void RegisterOrLogin()
        {
            while (this.isLogged == false)
            {
                ConsoleWriter.Write(Environment.NewLine);
                Console.Write(PromptingMessages.RegisterOrLogin);
                string command = Console.ReadLine();

                try
                {
                    switch (command.ToLower())
                    {
                        case "register":
                            this.Register();
                            break;

                        case "login":
                            this.Login();
                            break;

                        case "exit":
                            this.Exit();
                            break;

                        default:
                            throw new ArgumentException(ErrorMessages.InvalidCommand);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.Write(Environment.NewLine);
                    ConsoleWriter.WriteLine(e.Message);
                }
            }
        }

        private void InterpretCommand()
        {
            while (this.isLogged)
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.Write(PromptingMessages.PromptForCommand);
                string command = Console.ReadLine();
                ConsoleWriter.Write(Environment.NewLine);

                try
                {
                    switch (command.ToLower())
                    {
                        case "list-patients":
                            this.ListPatients();
                            break;

                        case "add-patient":
                            this.AddPatient();
                            break;

                        case "find-patient-id":
                            this.FindPatientById();
                            break;

                        case "find-patient-name":
                            this.FindPatientByName();
                            break;

                        case "list-visitations":
                            this.ListVisitations();
                            break;

                        case "add-visitation":
                            this.AddVisitation();
                            break;

                        case "list-diagnoses":
                            this.ListDiagnoses();
                            break;

                        case "add-diagnose":
                            this.AddDiagnose();
                            break;

                        case "list-medicaments":
                            this.ListMedicaments();
                            break;

                        case "add-medicament":
                            this.AddMedicament();
                            break;

                        case "list-prescriptions":
                            this.ListPrescriptions();
                            break;

                        case "add-prescription":
                            this.AddPrescription();
                            break;

                        case "logoff":
                            this.LogoffAndRedirectToEntryPoint();
                            break;

                        default:
                            throw new ArgumentException(ErrorMessages.InvalidCommand);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(e.Message);
                }
            }
        }

        private void Register()
        {
            ConsoleWriter.Write(Environment.NewLine);
            string name = Helpers.IsNullOrEmptyValidator("Name: ");
            string specialty = Helpers.IsNullOrEmptyValidator("Specialty: ");
            string username = Helpers.IsNullOrEmptyValidator("Username: ");

            ConsoleWriter.Write("Password: ");
            string password = Helpers.EnterHiddenPassword();

            if (this.context.Doctors.Any(d => d.Name.Equals(name) && d.Specialty.Equals(specialty)))
            {
                throw new ArgumentException(ErrorMessages.DoctorAlreadyExists);
            }

            Doctor doctor = new Doctor(name, specialty, username, password);

            this.context.Doctors.Add(doctor);
            this.context.SaveChanges();

            ConsoleWriter.Write(Environment.NewLine);
            ConsoleWriter.WriteLine(string.Format(InfoMessages.DoctorRegisteredSuccessfully, name, specialty, username));
        }

        private void Login()
        {
            ConsoleWriter.Write(Environment.NewLine);
            string username = Helpers.IsNullOrEmptyValidator("Username: ");

            ConsoleWriter.Write("Password: ");
            string password = Helpers.EnterHiddenPassword();
            ConsoleWriter.Write(Environment.NewLine);

            Doctor doctor = this.context.Doctors.FirstOrDefault(d => d.Username.Equals(username));

            if (doctor == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidCredentials);
            }

            string hashedPassword = Authenticate.GetHash(password + doctor.Salt);

            if (!hashedPassword.Equals(doctor.Password))
            {
                throw new ArgumentException(ErrorMessages.InvalidCredentials);
            }

            this.isLogged = true;
            this.loggedDoctorId = doctor.DoctorId;

            ConsoleWriter.Write(Environment.NewLine);
            ConsoleWriter.WriteLine($"Welcome Dr. {doctor.Name} ({doctor.Specialty})!");
        }

        private void Exit()
        {
            ConsoleWriter.Write(Environment.NewLine);
            bool wasExitConfirmed = Helpers.ValidateBoolEntered(PromptingMessages.ExitConfirmation);

            if (wasExitConfirmed)
            {
                Environment.Exit(0);
            }
        }

        private void ListPatients()
        {
            Patient[] patients = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
                .ToArray();

            this.PrintCollection(patients);
        }

        private void AddPatient()
        {
            Patient patient = this.ReadPatientInfoAndCreatePatient();
            this.context.Patients.Add(patient);
            this.context.SaveChanges();

            bool shouldAddVisitations = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldCollectionEntitiesBeAdded, "visitations"));

            while (shouldAddVisitations)
            {
                this.AddVisitation(patient.PatientId);
                ConsoleWriter.Write(Environment.NewLine);
                shouldAddVisitations = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldMoreCollectionEntitiesBeAdded, "visitations"));
            }

            bool shouldAddDiagnoses = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldCollectionEntitiesBeAdded, "diagnoses"));

            while (shouldAddDiagnoses)
            {
                this.AddDiagnose(patient.PatientId);
                ConsoleWriter.Write(Environment.NewLine);
                shouldAddDiagnoses = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.ShouldMoreCollectionEntitiesBeAdded, "diagnoses"));
            }

            ConsoleWriter.WriteLine(string.Format(InfoMessages.SuccessfullyAddedEntity, nameof(patient), patient.ToString()));
        }

        private void FindPatientById()
        {
            Patient searchedPatient = this.TryFindPatientById();
            ConsoleWriter.Write(Environment.NewLine);
            ConsoleWriter.WriteLine(searchedPatient);
        }

        private void FindPatientByName()
        {
            Patient[] searchedPatients = this.TryFindPatientByName();
            ConsoleWriter.Write(Environment.NewLine);

            foreach (Patient searchedPatient in searchedPatients)
            {
                Console.WriteLine(searchedPatient);
            }
        }

        private Patient[] TryFindPatientByName()
        {
            string firstName = Helpers.IsNullOrEmptyValidator("First name: ");
            string lastName = Helpers.IsNullOrEmptyValidator("Last name: ");

            Patient[] patients = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Medicament)
                .Where(p => p.FirstName.ToLower() == firstName.ToLower() && p.LastName.ToLower() == lastName.ToLower() && p.Visitations.Any(v => v.Doctor.DoctorId == this.loggedDoctorId))
                .ToArray();

            if (patients == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PatientWithNameNotFound, $"\"{firstName} {lastName}\""));
            }

            return patients;
        }

        private Patient ReadPatientInfoAndCreatePatient()
        {
            string firstName = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} first name: ");
            string lastName = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} last name: ");
            ConsoleWriter.Write($"{nameof(Patient)} address: ");
            string address = ConsoleReader.ReadLine();
            string email = Helpers.IsNullOrEmptyValidator($"{nameof(Patient)} email: ");
            bool hasInsurance = Helpers.ValidateBoolEntered(string.Format(PromptingMessages.PatientHasInsurance, nameof(Patient).ToLower()));

            Patient patient = new Patient(firstName, lastName, address, email, hasInsurance);
            return patient;
        }

        private void ListVisitations()
        {
            Visitation[] visitations = this.context.Doctors
                .Include(d => d.Visitations)
                .ThenInclude(v => v.Patient)
                .FirstOrDefault(d => d.DoctorId == this.loggedDoctorId)
                .Visitations
                .ToArray();

            this.PrintCollection(visitations);
        }

        private void PrintCollection<T>(T[] entities)
        {
            string collectionName = typeof(T).Name.ToLower() + "s";

            if (entities.Length == 0)
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionEmpty, collectionName));
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionIndicator, collectionName));
                ConsoleWriter.Write(Environment.NewLine);

                foreach (T entity in entities)
                {
                    ConsoleWriter.WriteLine($"  {entity}");
                }

                ConsoleWriter.Write(Environment.NewLine);
            }
        }

        private void AddVisitation()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            this.AddVisitation(patientId);
        }

        private void AddVisitation(int patientId)
        {
            DateTime date = this.TryParseDateInCertainFormat("dd/MM/yyyy HH:mm");

            ConsoleWriter.Write("Comments: ");
            string comments = ConsoleReader.ReadLine();

            Patient currentPatient = this.context.Patients.FirstOrDefault(p => p.PatientId == patientId);
            Doctor currentDoctor = this.context.Doctors.FirstOrDefault(d => d.DoctorId == this.loggedDoctorId);
            Visitation visitation = new Visitation(date, currentPatient, currentDoctor, comments);
            this.context.Visitations.Add(visitation);
            this.context.SaveChanges();

            ConsoleWriter.WriteLine(string.Format(InfoMessages.SuccessfullyAddedVisitation, visitation.Date, visitation.Patient.FirstName, visitation.Patient.LastName));
        }

        private DateTime TryParseDateInCertainFormat(string format)
        {
            string dateString = Helpers.IsNullOrEmptyValidator(PromptingMessages.VisitationDate);
            bool isEnteredValueDatetime = DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None,
                out DateTime date);

            while (!isEnteredValueDatetime)
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.InvalidFormattedDateInput, format));
                dateString = Helpers.IsNullOrEmptyValidator(PromptingMessages.VisitationDate);
                isEnteredValueDatetime = DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None,
                    out date);
            }

            return date;
        }

        private void ListDiagnoses()
        {
            Patient[] patients = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
                .ToArray();

            if (patients.Length == 0)
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionEmpty, "diagnoses"));
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                IList<Diagnose> diagnoses = new List<Diagnose>();

                foreach (Patient patient in patients)
                {
                    foreach (Diagnose diagnose in patient.Diagnoses)
                    {
                        diagnoses.Add(diagnose);
                    }
                }

                if (diagnoses.Count == 0)
                {
                    ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionEmpty, "diagnoses"));
                }
                else
                {
                    ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionIndicator, "diagnoses"));
                    ConsoleWriter.Write(Environment.NewLine);

                    foreach (Diagnose diagnose in diagnoses)
                    {
                        Console.WriteLine(diagnose);
                    }
                }

                ConsoleWriter.Write(Environment.NewLine);
            }
        }

        private void AddDiagnose()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            this.AddDiagnose(patientId);
        }

        private void AddDiagnose(int patientId)
        {
            Patient currentPatient = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .FirstOrDefault(p => p.PatientId == patientId);

            if (currentPatient == null)
            {
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.PatientWithIdNotFound, patientId));
                return;
            }

            if (!currentPatient.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.PatientDoesNotHaveVisitations, "diagnose"));
                ConsoleWriter.Write(Environment.NewLine);
                return;
            }

            string diagnoseName = Helpers.IsNullOrEmptyValidator("Diagnose name: ");
            string diagnoseComments = Helpers.IsNullOrEmptyValidator("Diagnose comments: ");

            Diagnose currentDiagnose = new Diagnose(diagnoseName, diagnoseComments, currentPatient);
            this.context.Diagnoses.Add(currentDiagnose);
            this.context.SaveChanges();

            ConsoleWriter.WriteLine(string.Format(InfoMessages.SuccessFullyAddedDiagnose, currentDiagnose.Name, currentPatient.FirstName, currentPatient.LastName));
        }

        private void ListMedicaments()
        {
            Medicament[] medicaments = this.context.Medicaments
                .Include(m => m.Prescriptions)
                .ToArray();

            if (medicaments.Length == 0)
            {
                ConsoleWriter.WriteLine("There are no medicaments.");
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                foreach (Medicament medicament in medicaments)
                {
                    ConsoleWriter.WriteLine($"  {medicament}");
                }

                ConsoleWriter.Write(Environment.NewLine);
            }
        }

        private void AddMedicament()
        {
            string medicamentName = Helpers.IsNullOrEmptyValidator("Medicament name: ");
            Medicament medicament = new Medicament(medicamentName);
            this.context.Medicaments.Add(medicament);
            this.context.SaveChanges();

            ConsoleWriter.WriteLine($"Medicament with name \"{medicament.Name}\" was added successfully.");
            ConsoleWriter.Write(Environment.NewLine);
        }

        private void ListPrescriptions()
        {
            Patient[] patients = this.context
                .Patients
                .Include(p => p.Visitations)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Medicament)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
                .ToArray();

            if (patients.Length == 0)
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionEmpty, "prescriptions"));
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                IList<PatientMedicament> prescriptions = new List<PatientMedicament>();   

                foreach (Patient patient in patients)
                {
                    foreach (PatientMedicament prescription in patient.Prescriptions)
                    {
                       prescriptions.Add(prescription);
                    }
                }

                // this.PrintCollection(prescriptions.ToArray());
                if (prescriptions.Count == 0)
                {
                    ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionEmpty, "prescriptions"));
                    ConsoleWriter.Write(Environment.NewLine);
                }
                else
                {
                    ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionIndicator, "prescriptions"));
                    ConsoleWriter.Write(Environment.NewLine);

                    foreach (PatientMedicament prescription in prescriptions)
                    {
                        Console.WriteLine(prescription);
                    }
                }
            }
        }

        private Patient TryFindPatientById()
        {
            int patientId = Helpers.TryIntParseInputString("Patient ID: ");

            Patient currentPatient = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Medicament)
                .FirstOrDefault(p => p.PatientId == patientId && p.Visitations.Any(v => v.Doctor.DoctorId == this.loggedDoctorId));

            if (currentPatient == null)
            {
                throw new ArgumentException(string.Format(ErrorMessages.PatientWithIdNotFound, patientId));
            }

            return currentPatient;
        }

        private Medicament TryFindMedicamentByName()
        {
            string medicamentName = Helpers.IsNullOrEmptyValidator("Medicament name: ");
            Medicament currentMedicament = this.context.Medicaments.FirstOrDefault(f => f.Name.Equals(medicamentName));

            if (currentMedicament == null)
            {
                throw new ArgumentNullException(string.Format(ErrorMessages.MedicamentNotFound, medicamentName));
            }

            return currentMedicament;
        }

        private PatientMedicament TryCreatePrescription(Patient patient, Medicament medicament)
        {
            if (this.context.Prescriptions.Any(p => p.Patient.PatientId == patient.PatientId && p.Medicament.MedicamentId == medicament.MedicamentId))
            {
                throw new InvalidOperationException(string.Format(ErrorMessages.PrescriptionAlreadyExists, medicament.Name, patient.FirstName, patient.LastName));
            }

            PatientMedicament prescription = new PatientMedicament(patient, medicament);
            return prescription;
        }

        private void AddPrescription()
        {
            Patient currentPatient = this.TryFindPatientById();

            if (!currentPatient.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.PatientDoesNotHaveVisitations, "prescription"));
                ConsoleWriter.Write(Environment.NewLine);
                return;
            }

            Medicament currentMedicament = this.TryFindMedicamentByName();

            PatientMedicament prescription = this.TryCreatePrescription(currentPatient, currentMedicament);

            this.context.Prescriptions.Add(prescription);
            this.context.SaveChanges();

            ConsoleWriter.Write(Environment.NewLine);
            ConsoleWriter.WriteLine(string.Format(InfoMessages.SuccessfullyPrescribedMedication, currentMedicament.Name, currentPatient.FirstName, currentPatient.LastName));
            ConsoleWriter.Write(Environment.NewLine);
        }

        private void LogoffAndRedirectToEntryPoint()
        {
            bool wasLogoffConfirmed = Helpers.ValidateBoolEntered(PromptingMessages.LogoffConfirmation);

            if (wasLogoffConfirmed)
            {
                this.isLogged = false;
                this.Run();
            }
        }
    }
}