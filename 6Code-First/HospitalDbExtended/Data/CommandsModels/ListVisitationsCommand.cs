using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ListVisitationsCommand : Command
    {
        public ListVisitationsCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Visitation[] visitations = this.Context.Doctors
                .Include(d => d.Visitations)
                .ThenInclude(v => v.Patient)
                .FirstOrDefault(d => d.DoctorId == this.LoggedDoctorId)
                .Visitations
                .ToArray();

            Helpers.PrintCollection(visitations);
        }
    }
}