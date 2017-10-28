using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyRoster
{
    public class Launcher
    {
        public static void Main()
        {
            int empsCount = int.Parse(Console.ReadLine());
            IList<Employee> employees = new List<Employee>();

            for (int i = 0; i < empsCount; i++)
            {
                string[] input = Console.ReadLine().Split();
                string name = input[0];
                decimal salary = decimal.Parse(input[1]);
                string position = input[2];
                string department = input[3];
                Employee currentEmployee = new Employee(name, salary, position, department);

                if (input.Length == 5)
                {
                    int age;
                    bool isNumber = int.TryParse(input[4], out age);

                    if (isNumber)
                    {
                        currentEmployee.Age = age;
                    }
                    else
                    {
                        currentEmployee.Email = input[4];
                    }
                }
                else if (input.Length == 6)
                {
                    currentEmployee.Email = input[4];
                    currentEmployee.Age = int.Parse(input[5]);
                }

                employees.Add(currentEmployee);
            }

            var highestAvgSalaryDept = employees.GroupBy(e => e.Department)
                .Select(gr =>
                new
                {
                    Dept = gr.Key,
                    Employees = gr.OrderByDescending(e => e.Salary),
                    AvgSalary = gr.Average(e => e.Salary)
                })
                .OrderByDescending(d => d.AvgSalary)
                .FirstOrDefault();

            Console.WriteLine($"Highest Average Salary: {highestAvgSalaryDept.Dept}");

            foreach (Employee employee in highestAvgSalaryDept.Employees)
            {
                Console.WriteLine(employee);
            }
        }
    }
}