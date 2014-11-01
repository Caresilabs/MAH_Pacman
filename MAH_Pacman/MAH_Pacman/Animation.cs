using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman
{
    public class Animation
    {
        public Point origin;

        public float stateTime;

        public int size;

        public int frames;

        public float frameDuration;

        public Animation(int x, int y, int size, int frames, float frameDuration)
        {
            this.origin = new Point(x, y);
            this.size = size;
            this.stateTime = 0;
            this.frames = frames;
            this.frameDuration = frameDuration;
        }
    }
}
