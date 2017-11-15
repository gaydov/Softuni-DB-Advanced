using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class AddMedicamentCommand : Command
    {
        public AddMedicamentCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            string medicamentName = Helpers.IsNullOrEmptyValidator("Medicament name: ");
            Medicament medicament = new Medicament(medicamentName);
            this.Context.Medicaments.Add(medicament);
            this.Context.SaveChanges();

            this.Writer.WriteLine($"Medicament with name \"{medicament.Name}\" was added successfully.");
        }
    }
}