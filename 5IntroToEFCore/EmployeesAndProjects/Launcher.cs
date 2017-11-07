using System;
using System.Globalization;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace EmployeesAndProjects
{
    public class Launcher
    {
        private const string DateFormat = "M/d/yyyy h:mm:ss tt";

        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var selectedEmployees = db.Employees
                    .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        ManagerFirstName = e.Manager.FirstName,
                        ManagerLastName = e.Manager.LastName,
                        Projects = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ep.Project.Name,
                                ep.Project.StartDate,
                                ep.Project.EndDate
                            })
                    })
                    .Take(30)
                    .ToList();

                foreach (var e in selectedEmployees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                    foreach (var p in e.Projects)
                    {
                        string projectName = p.Name;
                        string startDate = p.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture);
                        string endDate;

                        if (p.EndDate != null)
                        {
                            endDate = p.EndDate.Value.ToString(DateFormat, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            endDate = "not finished";
                        }

                        Console.WriteLine($"--{projectName} - {startDate} - {endDate}");
                    }
                }
            }
        }
    }
}