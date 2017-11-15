using System;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ExitCommand : Command
    {
        public ExitCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            bool wasExitConfirmed = Helpers.ValidateBoolEntered(PromptingMessages.ExitConfirmation);

            if (wasExitConfirmed)
            {
                Environment.Exit(0);
            }
        }
    }
}