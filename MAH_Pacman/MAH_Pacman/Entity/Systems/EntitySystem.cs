using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public abstract class EntitySystem
    {
        public GameEntity Entity { get; set; }

        public abstract void Init();

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch batch);

        public T GetComponent<T>()
        {
            return Entity.GetComponent<T>();
        }
    }
}
