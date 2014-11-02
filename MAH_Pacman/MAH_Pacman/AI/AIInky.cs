using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.AI
{
    public class AIInky : AIController
    {
        public AIInky(GameEntity ai, int x, int y)
            : base(ai, x, y)
        {
        }

        protected override void UpdateAIState(AIController.State state)
        {
            switch (state)
            {
                case State.SCATTER:
                    SetTarget(GetSpawn());
                    break;
                case State.CHASE:
                    TargetPacman();
                    break;
                case State.FRIGHTENED:
                case State.NORMAL:
                    TargetTile();
                    break;
                case State.DEAD:
                    SetTarget(GetSpawn());
                    break;
                default:
                    break;
            }
            base.UpdateAIState(state);
        }

        protected override void ChangedState(AIController.State state)
        {
        }

        private void TargetTile()
        {
            TransformationComponent pt = GetPacman().GetComponent<TransformationComponent>();
            PacmanComponent pp = GetPacman().GetComponent<PacmanComponent>();

            Vector2 target = new Vector2(pt.GetIntX() + pp.direction.X * 2, pt.GetIntY() + pp.direction.Y * 2);
            Vector2 delta = pt.position - target;
            delta *= 2;
            target = pt.position + delta;

            SetTarget((int)target.X, (int)target.Y);

            /* To locate Inky’s target, we first start by selecting the position 
             * two tiles in front of Pac-Man in his current direction of travel, similar to Pinky’s targeting method. 
             * From there, imagine drawing a vector from Blinky’s position to this tile, and then doubling the length of the vector.
             * The tile that this new, extended vector ends on will be Inky’s actual target.*/
        }

        protected override void TargetReached()
        {
            base.TargetReached();

            switch (GetState())
            {
                case State.SCATTER:
                    //TargetPacman();
                    // Scatter while we say so
                    break;
                case State.CHASE:
                    TargetPacman();
                    break;
                case State.FRIGHTENED:
                    SetTarget(GetSpawn());
                    break;
                case State.NORMAL:
                    TargetPacman();
                    break;
                case State.DEAD:
                    SetState(State.NORMAL);
                    GetEntity().GetComponent<TransformationComponent>().hasCollision = true;
                    break;
                default:
                    break;
            }
        }
    }
}
