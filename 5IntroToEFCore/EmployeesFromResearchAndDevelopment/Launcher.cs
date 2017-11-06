using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace EmployeesFromResearchAndDevelopment
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var selectedEmployees = db.Employees
                    .Where(e => e.Department.Name.Equals("Research and Development"))
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.Department,
                        e.Salary
                    });

                foreach (var employee in selectedEmployees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
                }
            }
        }
    }
}