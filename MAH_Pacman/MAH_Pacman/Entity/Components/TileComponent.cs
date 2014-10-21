using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class TileComponent : Component
    {
        public static float TILE_SIZE = 224 / (float)World.WIDTH;
        
        public static int PELLET_SCORE = 10;
    }
}
