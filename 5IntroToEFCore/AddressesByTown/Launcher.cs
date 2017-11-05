using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace AddressesByTown
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                List<Address> selectedAddresses = db.Addresses
                    .Include(a => a.Employees)
                    .Include(a => a.Town)
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .Take(10)
                    .ToList();

                foreach (Address address in selectedAddresses)
                {
                    Console.WriteLine($"{address.AddressText}, {address.Town.Name} - {address.Employees.Count} employees");
                }
            }
        }
    }
}