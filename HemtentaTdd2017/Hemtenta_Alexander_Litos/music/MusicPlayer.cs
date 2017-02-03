using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.music
{
    public class MusicPlayer : IMusicPlayer
    {
        private IMediaDatabase mockDb;
        private SoundMaker soundMaker;

        private List<ISong> playlist;

        public int NumSongsInQueue { get; set; } = 0;


        public MusicPlayer()
        {

        }
        public MusicPlayer(IMediaDatabase mockDb, SoundMaker soundMaker)
        {
            this.mockDb = mockDb;
            this.soundMaker = soundMaker;
            playlist = new List<ISong>();
        }

        public void LoadSongs(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                throw new NotImplementedException();
            }
            if (mockDb.IsConnected)
            {
                throw new DatabaseAlreadyOpenException();
            }
            if (mockDb.FetchSongs(search).Any((a => a.Title.Contains(search))))
            {
                mockDb.OpenConnection();

                playlist = mockDb.FetchSongs(search);
                NumSongsInQueue = playlist.Count;
            }
            if (!mockDb.IsConnected)
            {
                mockDb.CloseConnection();
                throw new DatabaseClosedException();
            }
        }

        public string NowPlaying()
        {
            if (NumSongsInQueue <= 0)
            {
                soundMaker.NowPlaying = "Tystnad råder";
                return soundMaker.NowPlaying;
            }
            soundMaker.NowPlaying = playlist.Last().Title;

            return soundMaker.NowPlaying;            
        }

        public void Play()
        {
            if (soundMaker.NowPlaying == "Tystnad råder")
            {
                soundMaker.IsPlay = true;
            }
        }

        public void Stop()
        {
            if (soundMaker.NowPlaying == "Agenda")
            {
                soundMaker.IsPlay = false;
            }
        }

        public void NextSong()
        {
            if (NumSongsInQueue <= 0)
            {
                if (playlist.Any())
                {
                    soundMaker.NowPlaying = playlist.Last().Title;
                }
                soundMaker.IsPlay = false;
                mockDb.CloseConnection();
            }
            while (NumSongsInQueue > 0)
            {
                NumSongsInQueue--;
                playlist.RemoveAt(NumSongsInQueue);
                soundMaker.IsPlay = true;

                break;
            }            
        }
    }
}
