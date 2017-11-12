using System;
using System.Globalization;
using System.Linq;
using HospitalDbExtended.Core.IO;
using HospitalDbExtended.Data;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

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
            this.LoginOrRegister();
        }

        public void LoginOrRegister()
        {
            while (this.isLogged == false)
            {
                ConsoleWriter.Write(Environment.NewLine);
                Console.Write("Do you want to login or register? ");
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

                        default:
                            throw new ArgumentException(ErrorMessages.InvalidInput);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(e.Message);
                }
            }

            this.InterpretCommand();
        }

        private void InterpretCommand()
        {
            while (this.isLogged == true)
            {
                ConsoleWriter.Write("Please enter command(type \"help\" to list all commands): ");
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
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(e.Message);
                    ConsoleWriter.Write(Environment.NewLine);
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
                throw new ArgumentNullException(ErrorMessages.DoctorAlreadyExists);
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
            ConsoleWriter.Write(Environment.NewLine);
        }

        private void ListPatients()
        {
            Patient[] patients = this.context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
                .ToArray();

            if (patients.Length == 0)
            {
                ConsoleWriter.WriteLine("You don't have any patients.");
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                ConsoleWriter.WriteLine("Your patients:");
                ConsoleWriter.Write(Environment.NewLine);

                foreach (Patient patient in patients)
                {
                    ConsoleWriter.WriteLine($"  {patient}");
                }
            }
        }

        private void AddPatient()
        {
            string firstName = Helpers.IsNullOrEmptyValidator("Patient first name: ");
            string lastName = Helpers.IsNullOrEmptyValidator("Patient last name: ");
            ConsoleWriter.Write("Patient address: ");
            string address = ConsoleReader.ReadLine();
            string email = Helpers.IsNullOrEmptyValidator("Patient email: ");
            bool hasInsurance = Helpers.ValidateBoolEntered("Does the patient has insurance? (Y/N): ");

            Patient patient = new Patient(firstName, lastName, address, email, hasInsurance);
            this.context.Patients.Add(patient);
            this.context.SaveChanges();

            bool shouldAddVisitations = Helpers.ValidateBoolEntered("Would you like to enter visitations for this patient? (Y/N): ");

            while (shouldAddVisitations)
            {
                this.AddVisitation(patient.PatientId);
                ConsoleWriter.Write(Environment.NewLine);
                shouldAddVisitations = Helpers.ValidateBoolEntered("Would you like to enter more visitations for this patient? (Y/N): ");
            }

            bool shouldAddDiagnoses = Helpers.ValidateBoolEntered("Would you like to enter diagnoses for this patient? (Y/N): ");

            while (shouldAddDiagnoses)
            {
                this.AddDiagnose(patient.PatientId);
                ConsoleWriter.Write(Environment.NewLine);
                shouldAddDiagnoses = Helpers.ValidateBoolEntered("Would you like to enter more diagnoses for this patient? (Y/N): ");
            }

            ConsoleWriter.WriteLine($"Successfully added patient {patient}.");
        }

        private void ListVisitations()
        {
            Visitation[] visitations = this.context.Doctors
                .Include(d => d.Visitations)
                .ThenInclude(v => v.Patient)
                .FirstOrDefault(d => d.DoctorId == this.loggedDoctorId)
                .Visitations
                .ToArray();

            ConsoleWriter.Write(Environment.NewLine);

            if (visitations.Length == 0)
            {
                ConsoleWriter.WriteLine("You have no visitations.");
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                ConsoleWriter.WriteLine("Your visitations:");
                ConsoleWriter.Write(Environment.NewLine);

                foreach (Visitation visitation in visitations)
                {
                    ConsoleWriter.WriteLine($"  {visitation}");
                }

                ConsoleWriter.Write(Environment.NewLine);
            }
        }

        private void AddVisitation()
        {
            bool canBeParsed = int.TryParse(Helpers.IsNullOrEmptyValidator("Patient ID: "), out int patientId);
            while (!canBeParsed)
            {
                ConsoleWriter.WriteLine(Environment.NewLine);
                ConsoleWriter.WriteLine("Invalid input. Please enter an integer number.");
                canBeParsed = int.TryParse(Helpers.IsNullOrEmptyValidator("Patient ID: "), out patientId);
            }

            this.AddVisitation(patientId);
        }

        private void AddVisitation(int patientId)
        {
            string dateStr = Helpers.IsNullOrEmptyValidator("Visitation date (format DD/MM/YYYY HH:MM): ");
            bool canBeParsed = DateTime.TryParseExact(dateStr, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out DateTime date);

            while (!canBeParsed)
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine("Invalid input. Please enter date in the format DD/MM/YYYY HH:MM.");
                dateStr = Helpers.IsNullOrEmptyValidator("Visitation date (format DD/MM/YYYY HH:MM): ");
                canBeParsed = DateTime.TryParseExact(dateStr, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out date);
            }

            ConsoleWriter.Write("Comments: ");
            string comments = ConsoleReader.ReadLine();

            Patient currentPatient = this.context.Patients.FirstOrDefault(p => p.PatientId == patientId);
            Doctor currentDoctor = this.context.Doctors.FirstOrDefault(d => d.DoctorId == this.loggedDoctorId);
            Visitation visitation = new Visitation(date, currentPatient, currentDoctor, comments);
            this.context.Visitations.Add(visitation);
            this.context.SaveChanges();

            ConsoleWriter.WriteLine($"Visitation with date {date:dd/MM/yyyy HH:mm} for {currentPatient.FirstName} {currentPatient.LastName} was added.");
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

            if (!patients.Any())
            {
                ConsoleWriter.WriteLine("You don't have any diagnoses.");
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                ConsoleWriter.WriteLine("Your diagnoses:");
                ConsoleWriter.Write(Environment.NewLine);

                foreach (Patient patient in patients)
                {
                    foreach (var diag in patient.Diagnoses)
                    {
                        Console.WriteLine(diag);
                    }
                }

                ConsoleWriter.Write(Environment.NewLine);
            }
        }

        private void AddDiagnose()
        {
            bool canBeParsed = int.TryParse(Helpers.IsNullOrEmptyValidator("Patient ID: "), out int patientId);
            while (!canBeParsed)
            {
                ConsoleWriter.WriteLine(Environment.NewLine);
                ConsoleWriter.WriteLine("Invalid input. Please enter an integer number.");
                canBeParsed = int.TryParse(Helpers.IsNullOrEmptyValidator("Patient ID: "), out patientId);
            }

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
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.PatientNotFound, patientId));
                return;
            }

            if (!currentPatient.Visitations.Any(v => v.DoctorId == this.loggedDoctorId))
            {
                ConsoleWriter.WriteLine(ErrorMessages.PatientDoesNotHaveVisitations);
                return;
            }

            string diagnoseName = Helpers.IsNullOrEmptyValidator("Diagnose name: ");
            string diagnoseComments = Helpers.IsNullOrEmptyValidator("Diagnose comments: ");

            Diagnose currentDiagnose = new Diagnose(diagnoseName, diagnoseComments, currentPatient);
            this.context.Diagnoses.Add(currentDiagnose);
            this.context.SaveChanges();

            ConsoleWriter.WriteLine($"Diagnose with name {diagnoseName} was added for \"{currentPatient.FirstName} {currentPatient.LastName}\".");
        }
    }
}