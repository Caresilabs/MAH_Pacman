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
    public class CollisionSystem : EntitySystem
    {
        private World world;

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(TransformationComponent));
        }

        public CollisionSystem(World world)
        {
            this.world = world;
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            for (int j = 0; j < entities.Count; j++)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    GameEntity e1 = entities[j];
                    GameEntity e2 = entities[i];

                    if (e1 == e2 ||
                        !e1.GetComponent<TransformationComponent>().hasCollision ||
                        !e2.GetComponent<TransformationComponent>().hasCollision)
                        continue;

                    if (Overlaps(e1.GetComponent<TransformationComponent>(), e2.GetComponent<TransformationComponent>()))
                    {
                        world.OnCollision(e1, e2);
                    }
                }
            }
        }

        private bool Overlaps(TransformationComponent t1, TransformationComponent t2)
        {
            return (t1.GetIntX() == t2.GetIntX() && t1.GetIntY() == t2.GetIntY());
            //return p1.X < p2.X + s2.X && p1.X + s1.X > s2.X && p1.Y < p2.Y + s2.Y && p1.Y + s1.Y > s2.Y;
        }
    }
}
