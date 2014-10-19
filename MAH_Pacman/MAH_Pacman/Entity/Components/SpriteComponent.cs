using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class SpriteComponent : Component
    {
        public Texture2D texture;

        public Rectangle source;

        public Rectangle bounds;

        public Vector2 origin;

        public double rotation;

        public SpriteComponent(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
