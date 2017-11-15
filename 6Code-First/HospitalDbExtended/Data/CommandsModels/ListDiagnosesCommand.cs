using System;
using System.Collections.Generic;
using System.Linq;
using HospitalDbExtended.Data.Interfaces;
using HospitalDbExtended.Data.Models;
using HospitalDbExtended.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HospitalDbExtended.Data.CommandsModels
{
    public class ListDiagnosesCommand : Command
    {
        public ListDiagnosesCommand(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
            : base(context, isDoctorLogged, loggedDoctorId, reader, writer)
        {
        }

        public override void Execute()
        {
            Patient[] patients = this.Context
                .Patients
                .Include(p => p.Visitations)
                .ThenInclude(v => v.Doctor)
                .Include(p => p.Diagnoses)
                .Where(p => p.Visitations.Any(v => v.DoctorId == this.LoggedDoctorId))
                .ToArray();

            if (patients.Length == 0)
            {
                this.Writer.WriteLine(string.Format(InfoMessages.ExtractedDoctorCollectionEmpty, "diagnoses"));
                this.Writer.Write(Environment.NewLine);
            }
            else
            {
                IList<Diagnose> diagnoses = new List<Diagnose>();

                foreach (Patient patient in patients)
                {
                    foreach (Diagnose diagnose in patient.Diagnoses)
                    {
                        diagnoses.Add(diagnose);
                    }
                }

                if (diagnoses.Count == 0)
                {
                    this.Writer.WriteLine(string.Format(InfoMessages.ExtractedDoctorCollectionEmpty, nameof(diagnoses)));
                }
                else
                {
                    this.Writer.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionIndicator, nameof(diagnoses)));
                    this.Writer.Write(Environment.NewLine);

                    foreach (Diagnose diagnose in diagnoses)
                    {
                        this.Writer.WriteLine(diagnose.ToString());
                        this.Writer.Write(Environment.NewLine);
                    }
                }
            }
        }
    }
}