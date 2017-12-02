using System.Collections.Generic;

namespace ProductsShop.Models.ModelDTOs
{
    public class UserWithSoldProductsDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<SoldProductDto> SoldProducts { get; set; }
    }
}