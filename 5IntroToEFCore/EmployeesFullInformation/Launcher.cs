using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace EmployeesFullInformation
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var employeesData = db.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.MiddleName,
                        e.JobTitle,
                        e.Salary
                    });

                foreach (var emp in employeesData)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:F2}");
                }
            }
        }
    }
}