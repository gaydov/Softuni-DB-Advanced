using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineRadioDatabase.Models
{
    public class Playlist
    {
        private readonly IList<Song> songs;

        public Playlist()
        {
            this.songs = new List<Song>();
        }

        public int SongsCount
        {
            get
            {
                return this.songs.Count;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                long totalLengthInSeconds = this.songs.Sum(s => s.Minutes * 60) + this.songs.Sum(s => s.Seconds);
                TimeSpan playlistLength = TimeSpan.FromSeconds(totalLengthInSeconds);
                return playlistLength;
            }
        }

        public void AddSong(Song song)
        {
            this.songs.Add(song);
        }
    }
}