using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class LoginCommand : Command
    {
        public LoginCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            string username = Helpers.IsNullOrEmptyValidator("Username: ");

            this.Writer.Write("Password: ");
            string password = Helpers.EnterPasswordHidden();
            this.Writer.Write(Environment.NewLine);

            Doctor doctor = this.Context.Doctors.FirstOrDefault(d => d.Username.Equals(username));

            if (doctor == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidCredentials);
            }

            string hashedPassword = PasswordHasher.GenerateHash(password + doctor.Salt);

            if (!hashedPassword.Equals(doctor.Password))
            {
                throw new ArgumentException(ErrorMessages.InvalidCredentials);
            }

            this.IsDoctorLogged = true;
            this.LoggedDoctorId = doctor.DoctorId;

            this.Writer.Write(Environment.NewLine);
            this.Writer.WriteLine($"Welcome Dr. {doctor.Name} ({doctor.Specialty})!");
        }
    }
}