using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsShop.App.Core;
using ProductsShop.Data;

namespace ProductsShop.App
{
    public class Launcher
    {
        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductsShopMappingProfile>());
            ProductsShopContext context = new ProductsShopContext();

            using (context)
            {
                ResetDatabase(context);
                CommandInterpreter commandInterpreter = new CommandInterpreter();
                Engine engine = new Engine(commandInterpreter, context);

                engine.Run();
            }
        }

        private static void ResetDatabase(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
