using Hemtenta_Alexander_Litos.bank;
using Hemtenta_Alexander_Litos.music;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hemtenta_Alexander_Litos_Tests
{
    public class MusicTests
    {
        //private string search;
        private MockDatabase mockMediaDb;
        private SoundMaker soundMaker;
        private MusicPlayer musicPlayer;
        private Song song;

        public MusicTests()
        {
            mockMediaDb = new MockDatabase();
            soundMaker = new SoundMaker();
            musicPlayer = new MusicPlayer(mockMediaDb, soundMaker);
            song = new Song { Title = "Agenda" };
        }

        [Fact]
        public void Should_LoadSongs_InvalidValues_Throws()
        {
            Assert.Throws<NotImplementedException>(() => musicPlayer.LoadSongs(null));
            Assert.Throws<NotImplementedException>(() => musicPlayer.LoadSongs(""));
        }

        [Fact]
        public void Should_Fail_Open_MediaDatabaseConnected_When_NoMatchSongInList()
        {
            Assert.Throws<DatabaseClosedException>(() => musicPlayer.LoadSongs("Pagenda"));
            Assert.Equal(false, mockMediaDb.IsConnected);
            Assert.Equal(0, musicPlayer.NumSongsInQueue);
        }

        [Fact]
        public void Should_Success_Open_MediaDatabaseConnected_When_MatchSongInList()
        {
            musicPlayer.LoadSongs(song.Title);

            Assert.Equal(3, musicPlayer.NumSongsInQueue);
            Assert.Equal(true, mockMediaDb.IsConnected);
        }

        [Fact]
        public void Should_LoadSongs_From_MockDB()
        {
            /*Try loadsongs from db if matchingsongs exist
            connection will open and song will load.
            This method contains two calls to mockdb*/
            musicPlayer.LoadSongs(song.Title);
            
            /*Checks if user success to connect to database
            First call is to check if user can connect to db
            next call is to fetch all songs*/
            Assert.Equal(2, mockMediaDb.HowManyTimesHasMockDbBeenCalled);
        }

        [Fact]
        public void Should_Count_LoadSongs_From_MockDB()
        {
            musicPlayer.LoadSongs(song.Title);

            Assert.Equal(3, musicPlayer.NumSongsInQueue);
        }

        [Fact]
        public void Should_Fail_MediaDatabaseConnected_Already_Open()
        {
            musicPlayer.LoadSongs(song.Title);

            Assert.Throws<DatabaseAlreadyOpenException>(() => musicPlayer.LoadSongs(song.Title));
        }

        [Fact]
        public void Should_Fail_Song_NowPlaying()
        {
            var nowPlaying = musicPlayer.NowPlaying();

            Assert.Equal("Tystnad råder", nowPlaying);
        }

        [Fact]
        public void Should_Success_Song_NowPlaying()
        {
            musicPlayer.LoadSongs(song.Title);
            var nowPlaying = musicPlayer.NowPlaying();

            Assert.Contains(song.Title, nowPlaying);
        }
        
        [Fact]
        public void Should_Success_Play_Song()
        {
            var nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.Play();

            Assert.Equal("Tystnad råder", nowPlaying);
            Assert.Equal(true, soundMaker.IsPlay);
        }

        [Fact]
        public void Should_Fail_Play_Song_Beacuse_OtherSong_NowPlaying()
        {
            musicPlayer.LoadSongs(song.Title);
            var nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.Play();

            Assert.Contains(song.Title, nowPlaying);
            Assert.Equal(false, soundMaker.IsPlay);
        }

        [Fact]
        public void Should_Success_Stop_Song()
        {
            musicPlayer.LoadSongs(song.Title);
            var nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.Stop();

            Assert.Contains(song.Title, nowPlaying);
            Assert.Equal(false, soundMaker.IsPlay);
        }

        [Fact]
        public void Should_Fail_Stop_Song_Beacuse_NoSong_NowPlaying()
        {
            var nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.Stop();

            Assert.Equal("Tystnad råder", nowPlaying);
            Assert.Equal(false, soundMaker.IsPlay);
        }
        
        [Fact]
        public void Should_Success_Play_NextSong_InQueue_Until_NoSong_Left_InQueue()
        {
            musicPlayer.LoadSongs(song.Title);

            string nowPlaying;

            nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.NextSong();
            Assert.Contains(song.Title, nowPlaying);
            Assert.Equal(true, soundMaker.IsPlay);

            nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.NextSong();
            Assert.Contains(song.Title, nowPlaying);
            Assert.Equal(true, soundMaker.IsPlay);

            nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.NextSong();
            Assert.Contains(song.Title, nowPlaying);
            Assert.Equal(true, soundMaker.IsPlay);

            nowPlaying = musicPlayer.NowPlaying();
            musicPlayer.NextSong();
            Assert.Contains("Tystnad råder", nowPlaying);
            Assert.Equal(false, soundMaker.IsPlay);
        }
    }
}
