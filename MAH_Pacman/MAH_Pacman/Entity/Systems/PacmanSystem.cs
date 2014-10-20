using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using MAH_Pacman.Scene2D;
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

        public override void Init() { }

        public override void Update(float delta)
        {
            GameEntity pacman = entities[0];
            PacmanComponent data = pacman.GetComponent<PacmanComponent>();
            MovementComponent movement = pacman.GetComponent<MovementComponent>();
            TransformationComponent transform = pacman.GetComponent<TransformationComponent>();

            if (engine.GetSystem<GridSystem>().GetTile(transform.GetIntX(), transform.GetIntY()).TryEatPellet())
            {
                data.score += TileComponent.PELLET_SCORE;
            }

            // Control pacman
            if (InputHandler.KeyReleased(Keys.W))
            {
                data.nextDirection = World.DIRECTION_UP;
            }
            if (InputHandler.KeyReleased(Keys.S))
            {
                data.nextDirection = World.DIRECTION_DOWN;
            }
            if (InputHandler.KeyReleased(Keys.A))
            {
                data.nextDirection = World.DIRECTION_LEFT;
            }
            if (InputHandler.KeyReleased(Keys.D))
            {
                data.nextDirection = World.DIRECTION_RIGHT;
            }

            // Check if he is allowed to turn and has the position to move
            if (data.nextDirection != World.DIRECTION_NONE && engine.GetSystem<GridSystem>().CanTurn(pacman, data.direction) && data.nextDirection != data.direction)
            {
                // If it is walkable
                if (engine.GetSystem<GridSystem>().IsWalkable(pacman, data.nextDirection))
                {
                    movement.velocity = new Vector2(PacmanComponent.SPEED * data.nextDirection.X,
                        PacmanComponent.SPEED * data.nextDirection.Y);
                    pacman.GetComponent<SpriteComponent>().rotation = GetRotation(data.nextDirection);

                    transform.position.X = (int)(transform.position.X + .5f);
                    transform.position.Y = (int)(transform.position.Y + .5f);

                    movement.halt = false;

                    data.direction = data.nextDirection;
                    data.nextDirection = World.DIRECTION_NONE;
                }
            }
        }

        public double GetRotation(Point direction)
        {
            if (Point.Equals(direction, World.DIRECTION_UP)) return -Math.PI / 2;
            else if (Point.Equals(direction, World.DIRECTION_DOWN)) return Math.PI / 2;
            else if (Point.Equals(direction, World.DIRECTION_LEFT)) return Math.PI;
            else if (Point.Equals(direction, World.DIRECTION_RIGHT)) return 0;
            return 0;
        }
    }
}
