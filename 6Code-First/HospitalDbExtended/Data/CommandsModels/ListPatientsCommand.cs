using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ListPatientsCommand : Command
    {
        public ListPatientsCommand(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Patient[] patients = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
                .ToArray();

            Helpers.PrintCollection(patients);
        }
    }
}