using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public abstract class IteratingSystem : EntitySystem
    {
        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            foreach (var entity in entities)
            {
                ProcessEntity(entity, delta);
            }
        }

        public abstract void ProcessEntity(GameEntity entity, float delta);
    }
}
