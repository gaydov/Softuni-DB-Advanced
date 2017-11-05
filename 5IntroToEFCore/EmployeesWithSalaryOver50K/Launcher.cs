﻿using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace EmployeesWithSalaryOver50K
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var employeesNames = db.Employees
                    .Where(e => e.Salary > 50000)
                    .Select(e => e.FirstName)
                    .OrderBy(n => n);

                foreach (string name in employeesNames)
                {
                    Console.WriteLine(name);
                }
            }
        }
    }
}