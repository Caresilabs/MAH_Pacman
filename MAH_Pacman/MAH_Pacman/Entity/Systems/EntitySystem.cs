using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public abstract class EntitySystem
    {
        public List<GameEntity> entities;

        public Engine engine;

        public EntitySystem()
        {
             this.entities = new List<GameEntity>();
        }

        public abstract void Init();

        public abstract Type[] RequeredComponents();

        protected Type[] FamilyFor(params Type[] type)
        {
            return type;
        }

        public abstract void Update(float delta);
    }
}
