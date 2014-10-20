using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class MovementComponent : Component
    {
        public Vector2 velocity;

        public bool halt = false;
    }
}
