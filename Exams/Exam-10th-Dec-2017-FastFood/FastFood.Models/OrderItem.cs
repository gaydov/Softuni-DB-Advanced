using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }

        [Required]
        public Order Order { get; set; }

        public int ItemId { get; set; }

        [Required]
        public Item Item { get; set; }

        [Required]
        [Range(0, int.MaxValue)]        
        public int Quantity { get; set; }
    }
}