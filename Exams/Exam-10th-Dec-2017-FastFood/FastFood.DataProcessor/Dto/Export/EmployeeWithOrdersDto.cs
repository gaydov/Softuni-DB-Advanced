using System.Collections.Generic;
using System.Linq;
using FastFood.DataProcessor.Dto.Import;

namespace FastFood.DataProcessor.Dto.Export
{
    public class EmployeeWithOrdersDto
    {
        public string Name { get; set; }

        public List<OrderDto> Orders { get; set; }

        public decimal TotalMade
        {
            get { return this.Orders.Sum(o => o.TotalPrice); }
        }
    }
}