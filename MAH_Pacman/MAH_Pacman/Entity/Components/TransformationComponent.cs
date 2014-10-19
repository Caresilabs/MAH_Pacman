using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class TransformationComponent : Component
    {
        public Vector2 position;

        public Vector2 size;

        public int GetIntX()
        {
            return (int)position.X;
        }

        public int GetIntY()
        {
            return (int)position.Y;
        }
    }
}
