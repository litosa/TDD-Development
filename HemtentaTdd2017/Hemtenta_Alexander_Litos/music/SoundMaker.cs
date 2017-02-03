using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.music
{
    public class SoundMaker : ISoundMaker
    {
        public string NowPlaying { get; set; } = "";
        public bool IsPlay { get; set; }

        public void Play(ISong song)
        {
            IsPlay = true;
        }

        public void Stop()
        {
            IsPlay = false;
        }
    }
}
