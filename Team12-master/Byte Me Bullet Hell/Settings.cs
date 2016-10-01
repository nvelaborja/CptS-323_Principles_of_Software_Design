using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Byte_Me_Bullet_Hell
{
    public class Settings
    {
        private int difficulty;
        private bool soundOn;

        public Settings(int Difficulty, bool SoundOn)
        {
            difficulty = Difficulty;
            soundOn = SoundOn;
        }

        public int Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        public bool SoundOn
        {
            get { return soundOn; }
            set { soundOn = value; }
        }

    }
}
