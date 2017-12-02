using System;
using System.Collections.Generic;

namespace ProductsShop.Models
{
    public class Category
    {
        private const int MinNameLength = 3;
        private const int MaxNameLength = 15;
        private string name;

        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value.Length < MinNameLength || value.Length > MaxNameLength)
                {
                    throw new ArgumentException($"The category's name should be between {MinNameLength} and {MaxNameLength} characters long.");
                }

                this.name = value;
            }
        }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}