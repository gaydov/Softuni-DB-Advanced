using System;
using System.Linq;
using PhotoShare.ClientExtended.Core.Commands;
using PhotoShare.Data;

namespace PhotoShare.ClientExtended.Core
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

                    Type[] allowedCommands = null;

                    if (Session.User == null)
                    {
                        allowedCommands = new Type[]
                        {
                            typeof(ListFriendsCommand),
                            typeof(RegisterUserCommand),
                            typeof(LoginCommand),
                            typeof(LogoutCommand),
                            typeof(ExitCommand)
                        };
                    }
                    else
                    {
                        allowedCommands = new Type[]
                        {
                            typeof(ModifyUserCommand),
                            typeof(AddFriendCommand),
                            typeof(AcceptFriendCommand),
                            typeof(AddTagCommand),
                            typeof(AddTownCommand),
                            typeof(CreateAlbumCommand),
                            typeof(ShareAlbumCommand),
                            typeof(UploadPictureCommand),
                            typeof(AddTagToCommand),
                            typeof(DeleteUserCommand),
                            typeof(ListFriendsCommand),
                            typeof(LogoutCommand),
                            typeof(ExitCommand)
                        };
                    }

                    if (allowedCommands.Contains(command.GetType()))
                    {
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
                    else
                    {
                        throw new InvalidOperationException("Invalid credentials!");
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
