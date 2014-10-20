using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
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

            if (movement.halt == false)
            {
                transform.position.X += movement.velocity.X * delta;
                transform.position.Y += movement.velocity.Y * delta;
            }

            // Check if entity is outside
            if (transform.GetIntX() < 0)
            {
                transform.position.X = World.WIDTH - .55f;
            }
            else if (transform.GetIntY() < 0)
            {
                transform.position.Y = World.HEIGHT -1;
            }
            else if (transform.GetIntX() >= World.WIDTH)
            {
                transform.position.X = -.4f;
            } 
            else if (transform.GetIntY() >= World.HEIGHT) {
                transform.position.Y += .5f;
                transform.position.Y %= World.HEIGHT;
            }


            GridSystem grid = engine.GetSystem<GridSystem>();

            if (!grid.IsForwardWalkable(entity) && grid.HasWalkedHalf(movement.velocity, transform) && movement.halt == false)
            {
                movement.halt = true;
            }
        }
    }
}
