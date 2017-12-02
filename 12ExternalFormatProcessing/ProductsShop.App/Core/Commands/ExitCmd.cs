using System;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExitCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}