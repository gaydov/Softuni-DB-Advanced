using PhotoShare.Data;

namespace PhotoShare.ClientExtended.Core.Commands
{
    public abstract class Command
    {
        public abstract string Execute(string[] data, PhotoShareContext context);
    }
}