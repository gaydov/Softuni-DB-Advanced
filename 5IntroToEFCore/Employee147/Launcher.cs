using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

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

                Employee searchedEmployee = db.Employees
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .SingleOrDefault(e => e.EmployeeId == SearchedEmployeeId);

                if (searchedEmployee != null)
                {
                    Console.WriteLine($"{searchedEmployee.FirstName} {searchedEmployee.LastName} - {searchedEmployee.JobTitle}");

                    foreach (EmployeeProject employeeProject in searchedEmployee.EmployeesProjects.OrderBy(ep => ep.Project.Name))
                    {
                        Console.WriteLine($"{employeeProject.Project.Name}");
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