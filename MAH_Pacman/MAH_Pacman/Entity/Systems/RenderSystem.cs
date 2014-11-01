using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public abstract class RenderSystem : EntitySystem
    {
        public abstract void Draw(SpriteBatch batch);

        public override void Init()
        {}

        public override void Update(float delta)
        {}
    }
}
