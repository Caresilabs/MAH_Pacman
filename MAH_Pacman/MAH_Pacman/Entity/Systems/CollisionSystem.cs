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
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(TransformationComponent));
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            foreach (var e1 in entities)
            {
                foreach (var e2 in entities)
                {
                    if (e1 == e2) continue;

                    if (Overlaps(e1.GetComponent<TransformationComponent>().position, e1.GetComponent<TransformationComponent>().size,
                        e2.GetComponent<TransformationComponent>().position, e2.GetComponent<TransformationComponent>().size)) {
                    }
                }
            }
        }

        private bool Overlaps(Vector2 p1, Vector2 s1, Vector2 p2, Vector2 s2)
        {
            return p1.X < p2.X + s2.X && p1.X + s1.X > s2.X && p1.Y < p2.Y + s2.Y && p1.Y + s1.Y > s2.Y;
        }
    }
}
