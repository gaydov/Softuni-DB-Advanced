using System.Text;
using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class HelpCommand : Command
    {
        public HelpCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            if (this.IsDoctorLogged)
            {
                this.DisplayHelpLoginState();
            }
            else
            {
                this.DisplayHelpInLogoffState();
            }
        }

        private void DisplayHelpLoginState()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\"list-patients\" - Lists all of your patients with their visitations, diagnoses and prescriptions.");
            sb.AppendLine("\"add-patient\" - Adds a patient to your patients database.");
            sb.AppendLine("\"add-visitation\" - Adds a visitation information to a patient of yours.");
            sb.AppendLine("\"find-patient-by-id\" - Finds and displays information about a patient with the entered ID.");
            sb.AppendLine("\"find-patient-by-name\" - Finds and displays information about a patient with the entered first and last names.");
            sb.AppendLine("\"list-visitations\" - Lists all of your visitations.");
            sb.AppendLine("\"list-diagnoses\" - Lists all of your diagnoses.");
            sb.AppendLine("\"add-diagnose\" - Adds a diagnose to a patient of yours.");
            sb.AppendLine("\"list-medicaments\" - Lists all medicaments in the database.");
            sb.AppendLine("\"add-medicament\" - Adds medicament in the database.");
            sb.AppendLine("\"list-prescriptions\" - Lists all of your prescriptions.");
            sb.AppendLine("\"add-prescription\" - Adds a prescription to a patient of yours.");
            sb.Append("\"logoff\" - Logoff from the system.");

            this.Writer.WriteLine(sb.ToString());
        }

        private void DisplayHelpInLogoffState()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\"register\" - Register into the system with username and password.");
            sb.AppendLine("\"login\" - Login to the system with username and password.");
            sb.Append("\"exit\" - Exit the program.");

            this.Writer.WriteLine(sb.ToString());
        }
    }
}