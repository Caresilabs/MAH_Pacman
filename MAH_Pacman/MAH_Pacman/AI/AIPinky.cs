using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.AI
{
    public class AIPinky : AIController
    {
        public AIPinky(GameEntity ai, int x, int y)
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
                    TargetFrontOfPacman();
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

        private void TargetFrontOfPacman()
        {
            TransformationComponent pt = GetPacman().GetComponent<TransformationComponent>();
            PacmanComponent pp = GetPacman().GetComponent<PacmanComponent>();

            SetTarget(pt.GetIntX() + pp.direction.X * 4, pt.GetIntY() + pp.direction.Y * 4);
        }

        protected override void TargetReached()
        {
            base.TargetReached();

            switch (GetState())
            {
                case State.SCATTER:
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
