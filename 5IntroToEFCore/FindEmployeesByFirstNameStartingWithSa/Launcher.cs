using System;
using System.Collections.Generic;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace FindEmployeesByFirstNameStartingWithSa
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                List<Employee> searchedEmployees = db.Employees
                    .Where(e => e.FirstName.StartsWith("Sa"))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

                foreach (Employee emp in searchedEmployees)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:F2})");
                }
            }
        }
    }
}