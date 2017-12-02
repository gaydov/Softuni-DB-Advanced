using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public abstract class Command
    {
        public abstract string Execute(ProductsShopContext context);
    }
}