using System;
using System.Linq;
using PhotoShare.Client.Core.Commands;
using PhotoShare.Data;

namespace PhotoShare.Client.Core
{
    public class Engine
    {
        private readonly CommandDispatcher commandDispatcher;
        private readonly PhotoShareContext context;

        public Engine(CommandDispatcher commandDispatcher, PhotoShareContext context)
        {
            this.commandDispatcher = commandDispatcher;
            this.context = context;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine().Trim();
                    string[] data = input.Split(' ');
                    Command command = this.commandDispatcher.DispatchCommand(data);
                    string result = string.Empty;

                    if (command.GetType() == typeof(ExitCommand))
                    {
                        Console.WriteLine("Good bye!");
                    }

                    try
                    {
                        result = command.Execute(data.Skip(1).ToArray(), this.context);
                        Console.WriteLine(result);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException($"Command {data[0]} not valid!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
