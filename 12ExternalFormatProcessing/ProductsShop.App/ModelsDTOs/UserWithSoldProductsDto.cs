using System.Collections.Generic;

namespace ProductsShop.App.ModelsDTOs
{
    public class UserWithSoldProductsDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<SoldProductDto> SoldProducts { get; set; }
    }
}