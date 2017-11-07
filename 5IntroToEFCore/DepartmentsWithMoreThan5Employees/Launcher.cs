using System;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace DepartmentsWithMoreThan5Employees
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var selectedDepartments = db.Departments
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        d.Name,
                        d.Manager,
                        d.Employees
                    })
                    .ToList();

                foreach (var department in selectedDepartments)
                {
                    Console.WriteLine($"{department.Name} - {department.Manager.FirstName} {department.Manager.LastName}");

                    foreach (Employee emp in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                    {
                        Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                    }

                    Console.WriteLine(new string('-', 10));
                }
            }
        }
    }
}