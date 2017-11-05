using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

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
                List<Employee> selectedEmployees = db.Employees
                    .Include(e => e.Manager)
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(e => e.Project)
                    .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Take(30)
                    .ToList();

                foreach (Employee e in selectedEmployees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - Manager: {e.Manager.FirstName} {e.Manager.LastName}");

                    foreach (EmployeeProject ep in e.EmployeesProjects)
                    {
                        string projectName = ep.Project.Name;
                        string startDate = ep.Project.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture);
                        string endDate;

                        if (ep.Project.EndDate != null)
                        {
                            endDate = ep.Project.EndDate.Value.ToString(DateFormat, CultureInfo.InvariantCulture);
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