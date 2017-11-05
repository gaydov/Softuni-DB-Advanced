using System;
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

                var employeesAddresses = db.Employees
                    .Select(e => e.Address)
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .Select(e => e.AddressText);

                foreach (string empAddress in employeesAddresses)
                {
                    Console.WriteLine(empAddress);
                }
            }
        }
    }
}