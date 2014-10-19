using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class PacmanComponent : Component
    {
        public static float SIZE = 1;
        public static float SPEED = 2;

        public Point nextDirection = World.DIRECTION_NONE;
    }
}
