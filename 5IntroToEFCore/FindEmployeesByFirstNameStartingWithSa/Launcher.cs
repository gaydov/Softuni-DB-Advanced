using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace FindEmployeesByFirstNameStartingWithSa
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var searchedEmployees = db.Employees
                    .Where(e => e.FirstName.StartsWith("Sa"))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary
                    })
                    .ToList();

                foreach (var emp in searchedEmployees)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:F2})");
                }
            }
        }
    }
}