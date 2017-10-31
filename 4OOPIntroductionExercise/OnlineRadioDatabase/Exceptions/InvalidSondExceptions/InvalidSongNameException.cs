using OnlineRadioDatabase.Utilities;

namespace OnlineRadioDatabase.Exceptions.InvalidSondExceptions
{
    public class InvalidSongNameException : InvalidSongException
    {
        public override string Message
        {
            get { return $"Song name should be between {Constants.SongNameMinLength} and {Constants.SongNameMaxLength} symbols."; }
        }
    }
}