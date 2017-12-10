using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FastFood.Models.Enums;

namespace FastFood.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public OrderType Type { get; set; }

        [Required]
        public decimal TotalPrice
        {
            get { return this.OrderItems.Select(oi => oi.Item.Price * oi.Quantity).Sum(); }
        }

        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}