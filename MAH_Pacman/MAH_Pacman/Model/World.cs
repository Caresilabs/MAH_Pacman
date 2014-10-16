using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Entity.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Model
{
    public class World
    {
        private Engine engine;

        public World()
        {
            this.engine = new Engine();
            initEntities();
        }

        Pacman pacman;
        private void initEntities()
        {
            pacman = new Pacman();
            pacman.Add(new MovementComponent());
            pacman.Add(new PositionComponent());
            pacman.Add(new SpriteComponent(Assets.getItems()));
            engine.Add(pacman);


            engine.Add(new RenderSystem());
        }

        public void Update(float delta)
        {
            engine.Update(delta);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            engine.Draw(batch);
            batch.End();
        }
    }
}
