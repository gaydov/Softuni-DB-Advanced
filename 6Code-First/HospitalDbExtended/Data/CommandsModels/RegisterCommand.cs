using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class RegisterCommand : Command
    {
        public RegisterCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            string name = Helpers.IsNullOrEmptyValidator("Names: ");
            string specialty = Helpers.IsNullOrEmptyValidator("Specialty: ");
            string username = Helpers.IsNullOrEmptyValidator("Username: ");

            if (this.Context.Doctors.Any(d => d.Username.Equals(username)))
            {
                this.Writer.Write(Environment.NewLine);
                this.Writer.WriteLine(string.Format(ErrorMessages.UsernameAlreadyExists, username));
                this.Writer.Write(Environment.NewLine);
                username = Helpers.IsNullOrEmptyValidator("Username: ");
            }

            this.Writer.Write("Password: ");
            string password = Helpers.EnterPasswordHidden();
            Doctor doctor = new Doctor(name, specialty, username, password);

            this.Context.Doctors.Add(doctor);
            this.Context.SaveChanges();

            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine(string.Format(InfoMessages.DoctorRegisteredSuccessfully, name, specialty, username));
        }
    }
}