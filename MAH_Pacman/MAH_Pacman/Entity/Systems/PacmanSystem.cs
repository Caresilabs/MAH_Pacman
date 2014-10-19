using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class PacmanSystem : EntitySystem
    {
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(PacmanComponent), typeof(MovementComponent));
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            GameEntity pacman = entities[0];
            PacmanComponent data = pacman.GetComponent<PacmanComponent>();
            MovementComponent movement = pacman.GetComponent<MovementComponent>();
            TransformationComponent transform = pacman.GetComponent<TransformationComponent>();

            if (engine.GetSystem<GridSystem>().GetTile(transform.GetIntX(), transform.GetIntY()).TryEatPellet())
            {
                //
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // pacman.GetComponent<MovementComponent>().velocity = new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                data.nextDirection = World.DIRECTION_UP;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                 data.nextDirection = World.DIRECTION_DOWN;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                 data.nextDirection = World.DIRECTION_LEFT;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                 data.nextDirection = World.DIRECTION_RIGHT;
            }

            // If pacman can turn
            if (data.nextDirection != World.DIRECTION_NONE && engine.GetSystem<GridSystem>().CanTurn(pacman, pacman.GetComponent<PacmanComponent>().nextDirection))
            {
                movement.velocity = new Vector2(PacmanComponent.SPEED * data.nextDirection.X,
                    PacmanComponent.SPEED * data.nextDirection.Y);
                pacman.GetComponent<SpriteComponent>().rotation = Math.PI;

                data.nextDirection = World.DIRECTION_NONE;
            }
        }
    }
}
