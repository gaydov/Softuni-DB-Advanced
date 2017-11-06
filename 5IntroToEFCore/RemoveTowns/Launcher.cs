using System;
using System.Collections.Generic;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace RemoveTowns
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                string townToBeDeletedName = Console.ReadLine();
                List<Address> addressesFromTheTargetTown = db.Addresses
                    .Where(a => a.Town.Name.Equals(townToBeDeletedName))
                    .ToList();

                foreach (Employee employee in db.Employees)
                {
                    if (addressesFromTheTargetTown.Contains(employee.Address))
                    {
                        employee.Address = null;
                    }
                }

                db.Addresses.RemoveRange(addressesFromTheTargetTown);
                Town townToBeDeleted = db.Towns.SingleOrDefault(t => t.Name.Equals(townToBeDeletedName));
                db.Towns.Remove(townToBeDeleted);
                db.SaveChanges();

                int removedAddressesCount = addressesFromTheTargetTown.Count;
                string addressSingleOrPlural;
                string wasWere;
                if (removedAddressesCount > 1)
                {
                    addressSingleOrPlural = "addresses";
                    wasWere = "were";
                }
                else
                {
                    addressSingleOrPlural = "address";
                    wasWere = "was";
                }

                Console.WriteLine($"{removedAddressesCount} {addressSingleOrPlural} in {townToBeDeletedName} {wasWere} deleted");
            }
        }
    }
}