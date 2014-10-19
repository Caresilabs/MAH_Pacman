using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class MovementSystem : IteratingSystem
    {
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(MovementComponent), typeof(TransformationComponent));
        }

        public override void Init()
        {
        }

        public override void ProcessEntity(GameEntity entity, float delta)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent transform = entity.GetComponent<TransformationComponent>();

            transform.position.X += movement.velocity.X * delta;
            transform.position.Y += movement.velocity.Y * delta;

            GridSystem grid = engine.GetSystem<GridSystem>();

            if (! grid.IsForwardWalkable(entity))
            {
                movement.velocity.X = 0;
                movement.velocity.Y = 0;
            }
        }
    }
}
