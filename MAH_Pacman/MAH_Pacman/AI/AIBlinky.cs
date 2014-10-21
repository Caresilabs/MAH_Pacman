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
        public AIBlinky(GameEntity ai) : base(ai)
        {

        }
        protected override void ChangedState(AIController.State state)
        {
        }
    }
}
