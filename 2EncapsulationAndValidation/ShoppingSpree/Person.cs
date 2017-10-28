using System;
using System.Collections.Generic;

namespace ShoppingSpree
{
    public class Person
    {
        private readonly IList<Product> products;
        private string name;
        private decimal money;

        public Person(string name, decimal money)
        {
            this.Name = name;
            this.Money = money;
            this.products = new List<Product>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty");
                }

                this.name = value;
            }
        }

        public decimal Money
        {
            get
            {
                return this.money;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Money cannot be negative");
                }

                this.money = value;
            }
        }

        public void BuyProduct(Product product)
        {
            if (this.Money < product.Price)
            {
                throw new InvalidOperationException($"{this.Name} can\'t afford {product.Name}");
            }

            this.Money -= product.Price;
            this.products.Add(product);
        }

        public override string ToString()
        {
            if (this.products.Count > 0)
            {
                return $"{this.Name} - {string.Join(", ", this.products)}";
            }

            return $"{this.Name} - Nothing bought";
        }
    }
}