using System.Text;
using ProductsShop.Data;

namespace ProductsShop.App.Core.Commands
{
    public class HelpCmd : Command
    {
        public override string Execute(ProductsShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("import-users-json");
            sb.AppendLine("import-users-xml");
            sb.AppendLine("import-products-json");
            sb.AppendLine("import-products-xml");
            sb.AppendLine("import-categories-json");
            sb.AppendLine("import-categories-xml");

            sb.AppendLine("export-productsinrange-json");
            sb.AppendLine("export-productsinrange-xml");
            sb.AppendLine("export-soldproducts-json");
            sb.AppendLine("export-soldproducts-xml");
            sb.AppendLine("export-categories-json");
            sb.AppendLine("export-categories-xml");
            sb.AppendLine("export-usersandproducts-json");
            sb.AppendLine("export-usersandproducts-xml");
            sb.AppendLine("exit");

            return sb.ToString().TrimEnd();
        }
    }
}