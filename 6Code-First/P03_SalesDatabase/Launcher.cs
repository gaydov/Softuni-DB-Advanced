using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase
{
    public class Launcher
    {
        public static void Main()
        {
            SalesContext context = new SalesContext();

            using (context)
            {
                Seed(context);
            }
        }

        private static void Seed(SalesContext context)
        {
            Customer customer = new Customer
            {
                Name = "Stoqn Dimitrov",
                CreditCardNumber = "11aa-2211",
                Email = "stoqn_d@abv.bg"
            };

            context.Customers.Add(customer);

            Product product = new Product
            {
                Name = "Table",
                Price = 124.99m,
                Description = "Great wooden table",
                Quantity = 2
            };

            context.Products.Add(product);

            Store store = new Store
            {
                Name = "IKEA"
            };

            context.Stores.Add(store);

            Sale sale = new Sale
            {
                Product = product,
                Store = store,
                Customer = customer
            };

            context.Sales.Add(sale);

            context.SaveChanges();
        }
    }
}