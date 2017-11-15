using System;
using System.Linq;
using HospitalDbExtended.Core.IO;
using HospitalDbExtended.Data;
using HospitalDbExtended.Data.CommandsModels;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Core
{
    public class Engine
    {
        private readonly IReader reader;
        private readonly IWriter writer;
        private readonly HospitalContext context;
        private bool isDoctorLogged;
        private int loggedDoctorId;

        public Engine(HospitalContext context)
        {
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
            this.context = context;
            this.isDoctorLogged = false;
        }

        public void Run()
        {
            this.StartSession();
            this.InterpretCommand();
        }

        private void StartSession()
        {
            while (this.isDoctorLogged == false)
            {
                this.writer.Write(Environment.NewLine);
                this.writer.Write(PromptingMessages.RegisterOrLogin);

                Type[] allowedCommands =
                {
                    typeof(RegisterCommand),
                    typeof(LoginCommand),
                    typeof(ExitCommand),
                    typeof(HelpCommand)
                };

                this.TryExecuteAllowedCommand(allowedCommands);
            }
        }

        private void InterpretCommand()
        {
            while (this.isDoctorLogged)
            {
                this.writer.Write(Environment.NewLine);
                this.writer.Write(PromptingMessages.PromptForCommand);

                Type[] allowedCommands =
                {
                    typeof(ListPatientsCommand),
                    typeof(AddPatientCommand),
                    typeof(AddVisitationCommand),
                    typeof(FindPatientByIdCommand),
                    typeof(FindPatientByNameCommand),
                    typeof(ListVisitationsCommand),
                    typeof(ListDiagnosesCommand),
                    typeof(AddDiagnoseCommand),
                    typeof(ListMedicamentsCommand),
                    typeof(AddMedicamentCommand),
                    typeof(ListPrescriptionsCommand),
                    typeof(AddPrescriptionCommand),
                    typeof(LogoffCommand),
                    typeof(HelpCommand)
                };

                this.TryExecuteAllowedCommand(allowedCommands);
            }

            this.Run();
        }

        private void TryExecuteAllowedCommand(Type[] allowedCommandsTypes)
        {
            try
            {
                string command = this.reader.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    throw new ArgumentException(ErrorMessages.NoCommandEntered);
                }

                this.writer.Write(Environment.NewLine);

                CommandGenerator cmdGenerator = new CommandGenerator();
                Command cmd = cmdGenerator.GenerateCommand(command, this.context, this.isDoctorLogged, this.loggedDoctorId, this.reader, this.writer);

                if (cmd == null)
                {
                    throw new ArgumentException(ErrorMessages.InvalidCommand);
                }

                if (!allowedCommandsTypes.Contains(cmd.GetType()))
                {
                    throw new ArgumentException(ErrorMessages.InvalidCommand);
                }

                cmd.Execute();
                this.isDoctorLogged = cmd.IsDoctorLogged;
                this.loggedDoctorId = cmd.LoggedDoctorId;
            }
            catch (Exception e)
            {
                this.writer.WriteLine(e.Message);
            }
        }
    }
}