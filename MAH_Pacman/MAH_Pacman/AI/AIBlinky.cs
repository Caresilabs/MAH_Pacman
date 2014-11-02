using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.AI
{
    public class AIBlinky : AIController
    {
        public AIBlinky(GameEntity ai, int x, int y) : base(ai, x, y)
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
                    TargetPacman();
                    break;
                case State.NORMAL:
                    TargetPacman();
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
