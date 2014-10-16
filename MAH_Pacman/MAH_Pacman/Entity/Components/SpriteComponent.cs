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
        public Texture2D Texture { get; set; }

        public Rectangle Source { get; set; }

        public Vector2 Origin { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
