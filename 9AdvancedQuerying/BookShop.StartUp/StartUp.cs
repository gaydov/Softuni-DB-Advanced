using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookShop.Data;
using BookShop.Models;

namespace BookShop
{
    public class StartUp
    {
        public static void Main()
        {
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            string[] booksByAgeRestriction = context.Books
                .Where(b => string.Equals(b.AgeRestriction.ToString(), command, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, booksByAgeRestriction);
            return result;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            const int CopiesHighCountLimit = 5000;

            string[] goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < CopiesHighCountLimit)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, goldenBooks);
            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            const decimal PriceLowValue = 40m;

            string[] booksByPrice = context.Books
                .Where(b => b.Price > PriceLowValue)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}")
                .ToArray();

            string result = string.Join(Environment.NewLine, booksByPrice);
            return result;
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            string[] booksNotReleasedInYear = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, booksNotReleasedInYear);
            return result;
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string[] booksByCategory = context.Books
                .Where(b => b.BookCategories.Select(c => c.Category.Name.ToLower()).Intersect(categories).Any())
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            string result = string.Join(Environment.NewLine, booksByCategory);
            return result;
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            string[] booksReleasedBefore = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", null))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType:G} - ${b.Price:F2}")
                .ToArray();

            string result = string.Join(Environment.NewLine, booksReleasedBefore);
            return result;
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            string[] authorsEndingWithString = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToArray();

            string result = string.Join(Environment.NewLine, authorsEndingWithString);
            return result;
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] booksTitleContainingInputString = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            string result = string.Join(Environment.NewLine, booksTitleContainingInputString);
            return result;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string[] booksAuthorsNameStartsWithString = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            string result = string.Join(Environment.NewLine, booksAuthorsNameStartsWithString);
            return result;
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksWihtTitleLongerThan = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return booksWihtTitleLongerThan;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            string[] authorsWithBooksCount = context.Authors
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    CopiesCount = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.CopiesCount)
                .Select(a => $"{a.Name} - {a.CopiesCount}")
                .ToArray();

            string result = string.Join(Environment.NewLine, authorsWithBooksCount);
            return result;
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            string[] profitsByCategories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .Select(c => $"{c.CategoryName} ${c.Profit:F2}")
                .ToArray();

            string result = string.Join(Environment.NewLine, profitsByCategories);
            return result;
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriestWithThreeMostRecentBooks = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BooksCount = c.CategoryBooks.Sum(bc => bc.Book.BookId),
                    Books = c.CategoryBooks.Select(bc => bc.Book).OrderByDescending(b => b.ReleaseDate).Take(3)
                })
                .OrderBy(c => c.CategoryName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var cat in categoriestWithThreeMostRecentBooks)
            {
                sb.AppendLine($"--{cat.CategoryName}");

                foreach (Book book in cat.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().Trim();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            const int ReleaseDate = 2010;
            const decimal IncreaseValue = 5;

            List<Book> booksWhichPriceWillBeIncreased = context.Books
                .Where(b => b.ReleaseDate.Value.Year < ReleaseDate)
                .ToList();

            booksWhichPriceWillBeIncreased.ForEach(b => b.Price += IncreaseValue);

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            const int CopiesHighValue = 4200;

            Book[] booksToBeRemoved = context.Books
                .Where(b => b.Copies < CopiesHighValue)
                .ToArray();

            int booksToBeRemovedCount = booksToBeRemoved.Length;

            context.Books.RemoveRange(booksToBeRemoved);
            context.SaveChanges();

            return booksToBeRemovedCount;
        }
    }
}
