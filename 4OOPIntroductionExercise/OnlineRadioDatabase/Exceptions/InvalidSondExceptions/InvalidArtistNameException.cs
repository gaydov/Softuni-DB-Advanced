using OnlineRadioDatabase.Utilities;

namespace OnlineRadioDatabase.Exceptions.InvalidSondExceptions
{
    public class InvalidArtistNameException : InvalidSongException
    {
        public override string Message
        {
            get { return $"Artist name should be between {Constants.ArtistNameMinLength} and {Constants.ArtistNameMaxLength} symbols."; }
        }
    }
}