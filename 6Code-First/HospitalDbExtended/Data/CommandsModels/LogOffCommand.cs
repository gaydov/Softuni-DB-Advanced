using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class LogoffCommand : Command
    {
        public LogoffCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            bool wasLogoffConfirmed = Helpers.ValidateBoolEntered(PromptingMessages.LogoffConfirmation);

            if (wasLogoffConfirmed)
            {
                this.IsLogged = false;
            }
        }
    }
}