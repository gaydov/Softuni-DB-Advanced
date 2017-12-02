namespace ProductsShop.App.ModelsDTOs
{
    public class CategoryDto
    {
        public string Category { get; set; }

        public int ProductsCount { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}