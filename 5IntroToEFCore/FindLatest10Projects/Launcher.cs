using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace FindLatest10Projects
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                List<Project> latestTenProjects = db.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .OrderBy(p => p.Name)
                    .ToList();

                foreach (Project project in latestTenProjects)
                {
                    Console.WriteLine(project.Name);
                    Console.WriteLine(project.Description);
                    Console.WriteLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}