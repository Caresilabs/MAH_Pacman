using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class AISystem : IteratingSystem
    {
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(AIComponent), typeof(MovementComponent));
        }

        public override void ProcessEntity(GameEntity entity, float delta)
        {
            AIComponent ai = entity.GetComponent<AIComponent>();
            ai.controller.Update(delta);
        }
    }
}
