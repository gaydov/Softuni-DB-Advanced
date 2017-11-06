using System;
using System.Collections.Generic;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace AddingNewAddressAndUpdatingEmployee
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                Address address = new Address
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                Employee employee = db.Employees.FirstOrDefault(e => e.LastName.Equals("Nakov"));
                employee.Address = address;
                db.SaveChanges();

                List<string> employeesAddresses = db.Employees
                    .OrderByDescending(e => e.Address.AddressId)
                    .Take(10)
                    .Select(e => e.Address.AddressText)
                    .ToList();

                foreach (string empAddress in employeesAddresses)
                {
                    Console.WriteLine(empAddress);
                }
            }
        }
    }
}