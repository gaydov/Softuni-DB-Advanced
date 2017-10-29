namespace BookShop.Models
{
    public class GoldenEditionBook : Book
    {
        public GoldenEditionBook(string author, string title, decimal price)
            : base(author, title, price)
        {
            this.Price *= 1.3m;
        }
    }
}