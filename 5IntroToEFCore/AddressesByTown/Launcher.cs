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
                var selectedAddresses = db.Addresses
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .Take(10)
                    .Select(a => new
                    {
                        Text = a.AddressText,
                        Town = a.Town.Name,
                        EmployeesCount = a.Employees.Count
                    })
                    .ToList();

                foreach (var address in selectedAddresses)
                {
                    Console.WriteLine($"{address.Text}, {address.Town} - {address.EmployeesCount} employees");
                }
            }
        }
    }
}