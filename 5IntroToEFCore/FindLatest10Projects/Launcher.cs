using System;
using System.Globalization;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace FindLatest10Projects
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var latestTenProjects = db.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .OrderBy(p => p.Name)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.StartDate
                    })
                    .ToList();

                foreach (var project in latestTenProjects)
                {
                    Console.WriteLine(project.Name);
                    Console.WriteLine(project.Description);
                    Console.WriteLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}