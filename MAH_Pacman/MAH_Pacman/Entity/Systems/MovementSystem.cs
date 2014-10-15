using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class MovementSystem : EntitySystem
    {
        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            MovementComponent movement = GetComponent<MovementComponent>();

            movement.Velocity.X += 1;

            Console.WriteLine(movement.Velocity.ToString());
        }

        public override void Draw(SpriteBatch batch)
        {
        }
    }
}
