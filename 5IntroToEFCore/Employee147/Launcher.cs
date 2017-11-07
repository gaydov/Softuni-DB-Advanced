using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace Employee147
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                const int SearchedEmployeeId = 147;

                var searchedEmployee = db.Employees
                      .Select(e => new
                      {
                          e.EmployeeId,
                          e.FirstName,
                          e.LastName,
                          e.JobTitle,
                          Projects = e.EmployeesProjects.Select(ep => new
                          {
                              ep.Project.Name
                          })
                      })
                      .SingleOrDefault(e => e.EmployeeId == SearchedEmployeeId);

                if (searchedEmployee != null)
                {
                    Console.WriteLine($"{searchedEmployee.FirstName} {searchedEmployee.LastName} - {searchedEmployee.JobTitle}");

                    foreach (var project in searchedEmployee.Projects.OrderBy(ep => ep.Name))
                    {
                        Console.WriteLine($"{project.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($"Employee with ID {SearchedEmployeeId} was not found.");
                }
            }
        }
    }
}