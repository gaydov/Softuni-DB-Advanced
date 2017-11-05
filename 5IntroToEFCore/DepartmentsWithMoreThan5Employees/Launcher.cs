using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace DepartmentsWithMoreThan5Employees
{
    public class Launcher
    {
        public static void Main()
        {
            // NOT WORKING IN JUDGE
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var departments = db.Departments
                    .Where(d => d.Employees.Count > 5)
                    .Include(d => d.Manager)
                    .Include(d => d.Employees)
                    .OrderBy(d => d.Employees.Count);

                foreach (Department department in departments)
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