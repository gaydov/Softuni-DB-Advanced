using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ListMedicamentsCommand : Command
    {
        public ListMedicamentsCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Medicament[] medicaments = this.Context.Medicaments
                .Include(m => m.Prescriptions)
                .ToArray();

            if (medicaments.Length == 0)
            {
                this.Writer.WriteLine(string.Format(InfoMessages.ExtractedCollectionEmpty, nameof(medicaments)));
                this.Writer.Write(Environment.NewLine);
            }
            else
            {
                foreach (Medicament medicament in medicaments)
                {
                    this.Writer.WriteLine($"  {medicament}");
                    this.Writer.Write(Environment.NewLine);
                }
            }
        }
    }
}