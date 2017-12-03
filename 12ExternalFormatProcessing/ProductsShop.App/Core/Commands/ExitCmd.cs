using System;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class ExitCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            bool wasExitConfirmed = Helpers.ValidateBoolEntered(Messages.ExitConfirmation);

            if (wasExitConfirmed)
            {
                Environment.Exit(0);
            }

            return "Exit cancelled.";
        }
    }
}