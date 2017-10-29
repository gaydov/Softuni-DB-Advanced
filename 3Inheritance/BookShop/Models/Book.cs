using System;
using System.Text;
using BookShop.Utilities;

namespace BookShop.Models
{
    public class Book
    {
        private string author;
        private string title;
        private decimal price;

        public Book(string author, string title, decimal price)
        {
            this.Title = title;
            this.Author = author;
            this.Price = price;
        }

        public string Author
        {
            get
            {
                return this.author;
            }

            protected set
            {
                string[] names = value.Split();

                if (names.Length > 1)
                {
                    char secondNameFirstLetter = names[1][0];
                    if (char.IsDigit(secondNameFirstLetter))
                    {
                        throw new ArgumentException(ErrorMessages.AuthorInvalidName);
                    }
                }

                this.author = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            protected set
            {
                if (value.Length < Constants.TitleMinLenght)
                {
                    throw new ArgumentException(ErrorMessages.TitleInvalidLenght);
                }

                this.title = value;
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }

            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ErrorMessages.PriceInvalid);
                }

                this.price = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type: {this.GetType().Name}");
            sb.AppendLine($"Title: {this.Title}");
            sb.AppendLine($"Author: {this.Author}");
            sb.Append($"Price: {this.Price:F2}");

            return sb.ToString();
        }
    }
}