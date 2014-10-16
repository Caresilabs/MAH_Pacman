using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public abstract class EntitySystem
    {
        public List<GameEntity> Entities = new List<GameEntity>();

        public Engine Engine { get; set; }

        public abstract void Init();

        public abstract Type[] RequeredComponents();

        protected Type[] Requered(params Type[] type)
        {
            return type;
        }

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch batch);
    }
}
