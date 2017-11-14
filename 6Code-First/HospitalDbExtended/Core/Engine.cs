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
        private bool isLogged;
        private int loggedDoctorId;

        public Engine(HospitalContext context)
        {
            this.context = context;
            this.isLogged = false;
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
        }

        public void Run()
        {
            this.RegisterOrLogin();
            this.InterpretCommand();
        }

        private void RegisterOrLogin()
        {
            while (this.isLogged == false)
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
            while (this.isLogged)
            {
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

                this.writer.Write(Environment.NewLine);
                this.writer.Write(PromptingMessages.PromptForCommand);

                this.TryExecuteAllowedCommand(allowedCommands);
            }

            this.Run();
        }

        private void TryExecuteAllowedCommand(Type[] allowedCommands)
        {
            try
            {
                string command = Console.ReadLine();
                this.writer.Write(Environment.NewLine);

                CommandGenerator cmdInterpreter = new CommandGenerator();
                Command cmd = cmdInterpreter.GenerateCommand(command, this.context, this.isLogged, this.loggedDoctorId,
                    this.reader,
                    this.writer);

                if (cmd == null)
                {
                    throw new ArgumentException(ErrorMessages.InvalidCommand);
                }

                if (!allowedCommands.Contains(cmd.GetType()))
                {
                    throw new ArgumentException(ErrorMessages.InvalidCommand);
                }

                cmd.Execute();
                this.isLogged = cmd.IsLogged;
                this.loggedDoctorId = cmd.LoggedDoctorId;
            }
            catch (Exception e)
            {
                this.writer.WriteLine(e.Message);
            }
        }
    }
}