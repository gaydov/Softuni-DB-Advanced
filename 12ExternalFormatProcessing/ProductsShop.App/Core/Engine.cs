using System;
using ProductsShop.App.Core.Commands;
using ProductsShop.Data;

namespace ProductsShop.App.Core
{
    public class Engine
    {
        private readonly CommandInterpreter cmdInterpreter;
        private readonly ProductsShopContext context;

        public Engine(CommandInterpreter commandInterpreter, ProductsShopContext context)
        {
            this.cmdInterpreter = commandInterpreter;
            this.context = context;
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("Enter command (\"help\" for all the commands): ");
                string inputCommand = Console.ReadLine();

                try
                {
                    Command command = this.cmdInterpreter.TryInterpretCommand(inputCommand);
                    string result = command.Execute(this.context);
                    Console.WriteLine(result);
                    Console.Write(Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}