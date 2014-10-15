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
        private List<GameEntity> entities;

        public World()
        {
            this.entities = new List<GameEntity>();
            initEntities();
        }

        private void initEntities()
        {
            Pacman pacman = new Pacman();
            pacman.AddComponent(new MovementComponent());
            pacman.AddSystem(new MovementSystem());

            entities.Add(pacman);
        }

        public void Update(float delta)
        {
            foreach (var system in entities)
            {
                system.Update(delta);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var system in entities)
            {
                system.Draw(batch);
            }
        }
    }
}
