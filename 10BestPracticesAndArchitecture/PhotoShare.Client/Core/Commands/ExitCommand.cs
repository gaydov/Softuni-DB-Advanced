using System;
using PhotoShare.Data;

namespace PhotoShare.Client.Core.Commands
{
    public class ExitCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}
