using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.music
{
    public class MockDatabase : IMediaDatabase
    {
        public bool IsConnected { get; set; }
        public List<ISong> SongList { get; set; }
        public int HowManyTimesHasMockDbBeenCalled { get; set; } = 0;

        public MockDatabase()
        {
            SongList = new List<ISong>();
            SongList.Add(new Song { Title = "Agenda 1" });
            SongList.Add(new Song { Title = "Agenda 2" });
            SongList.Add(new Song { Title = "Agenda 3" });
        }

        public void CloseConnection()
        {
            IsConnected = false;
        }

        public List<ISong> FetchSongs(string search)
        {
            HowManyTimesHasMockDbBeenCalled++;
            return SongList;
        }

        public void OpenConnection()
        {
            IsConnected = true;
        }
    }
}
