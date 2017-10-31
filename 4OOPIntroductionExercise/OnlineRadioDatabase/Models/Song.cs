using OnlineRadioDatabase.Exceptions.InvalidSondExceptions;
using OnlineRadioDatabase.Exceptions.InvalidSondExceptions.InvalidSongLengthExceptions;
using OnlineRadioDatabase.Utilities;

namespace OnlineRadioDatabase.Models
{
    public class Song
    {
        private string artist;
        private string name;
        private int minutes;
        private int seconds;

        public Song(string artist, string name, int minutes, int seconds)
        {
            this.Artist = artist;
            this.Name = name;
            this.Minutes = minutes;
            this.Seconds = seconds;
        }

        public string Artist
        {
            get
            {
                return this.artist;
            }

            private set
            {
                if (value.Length < Constants.ArtistNameMinLength || value.Length > Constants.ArtistNameMaxLength)
                {
                    throw new InvalidArtistNameException();
                }

                this.artist = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (value.Length < Constants.SongNameMinLength || value.Length > Constants.SongNameMaxLength)
                {
                    throw new InvalidSongNameException();
                }

                this.name = value;
            }
        }

        public int Minutes
        {
            get
            {
                return this.minutes;
            }

            private set
            {
                if (value < Constants.SongMinutesMinValue || value > Constants.SongMinutesMaxValue)
                {
                    throw new InvalidSongMinutesException();
                }

                this.minutes = value;
            }
        }

        public int Seconds
        {
            get
            {
                return this.seconds;
            }

            private set
            {
                if (value < 0 || value > 59)
                {
                    throw new InvalidSongSecondsException();
                }

                this.seconds = value;
            }
        }
    }
}