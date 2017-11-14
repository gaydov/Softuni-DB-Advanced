using System;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ListMedicamentsCommand : Command
    {
        public ListMedicamentsCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Medicament[] medicaments = this.Context.Medicaments
                .Include(m => m.Prescriptions)
                .ToArray();

            if (medicaments.Length == 0)
            {
                this.Writer.WriteLine("There are no medicaments.");
                this.Writer.Write(Environment.NewLine);
            }
            else
            {
                foreach (Medicament medicament in medicaments)
                {
                    this.Writer.WriteLine($"  {medicament}");
                }

                this.Writer.Write(Environment.NewLine);
            }
        }
    }
}