using OnlineRadioDatabase.Utilities;

namespace OnlineRadioDatabase.Exceptions.InvalidSondExceptions.InvalidSongLengthExceptions
{
    public class InvalidSongMinutesException : InvalidSongLengthException
    {
        public override string Message
        {
            get { return $"Song minutes should be between {Constants.SongMinutesMinValue} and {Constants.SongMinutesMaxValue}."; }
        }
    }
}