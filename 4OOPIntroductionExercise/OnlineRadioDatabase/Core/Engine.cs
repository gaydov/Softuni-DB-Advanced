using System;
using OnlineRadioDatabase.Exceptions;
using OnlineRadioDatabase.Exceptions.InvalidSondExceptions;
using OnlineRadioDatabase.Models;
using OnlineRadioDatabase.Utilities;

namespace OnlineRadioDatabase.Core
{
    public class Engine
    {
        private readonly Playlist playlist;

        public Engine()
        {
            this.playlist = new Playlist();
        }

        public void Run()
        {
            this.ProcessInput();
            this.PrintOutput();
        }

        private void ProcessInput()
        {
            int songsCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < songsCount; i++)
            {
                string[] songInfo = Console.ReadLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    if (songInfo.Length != 3)
                    {
                        throw new InvalidSongException();
                    }

                    string artistName = songInfo[0];
                    string songName = songInfo[1];
                    string length = songInfo[2];

                    string[] lengthArgs = length.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    int minutes;
                    int seconds;

                    if (int.TryParse(lengthArgs[0], out minutes) && int.TryParse(lengthArgs[1], out seconds))
                    {
                        Song song = new Song(artistName, songName, minutes, seconds);
                        this.playlist.AddSong(song);
                        Console.WriteLine(Constants.SongAddedConfirmationMessage);
                    }
                    else
                    {
                        throw new InvalidSongLengthException();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void PrintOutput()
        {
            Console.WriteLine($"Songs added: {this.playlist.SongsCount}");
            Console.WriteLine($"Playlist length: {this.playlist.Duration.Hours}h {this.playlist.Duration.Minutes}m {this.playlist.Duration.Seconds}s");
        }
    }
}