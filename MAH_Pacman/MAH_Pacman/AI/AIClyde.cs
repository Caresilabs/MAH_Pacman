using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.AI
{
    public class AIClyde : AIController
    {
        public AIClyde(GameEntity ai, int x, int y)
            : base(ai, x, y)
        {
        }

        protected override void UpdateAIState(AIController.State state)
        {
            switch (state)
            {
                case State.DEAD:
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

            if ((pt.position - GetEntity().GetComponent<TransformationComponent>().position).Length() >= 8)
            {
                SetTarget(pt.GetIntX(), pt.GetIntY());
            }
            else
            {
                SetTarget(GetSpawn());
            }
        }

        protected override void TargetReached()
        {
            base.TargetReached();

            switch (GetState())
            {
                case State.SCATTER:
                    break;
                case State.CHASE:
                case State.NORMAL:
                    TargetPacman();
                    break;
                case State.FRIGHTENED:
                    SetTarget(GetSpawn());
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
