using System;
using System.Collections.Generic;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace IncreaseSalaries
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                string[] targetedDepartments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

                List<Employee> targetedEmployees = db.Employees
                    .Where(e => targetedDepartments.Contains(e.Department.Name))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

                foreach (Employee emp in targetedEmployees)
                {
                    emp.Salary *= 1.12m;
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:F2})");
                }

                db.SaveChanges();
            }
        }
    }
}