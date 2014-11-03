using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class AISystem : IteratingSystem
    {
        private float frightTime;
        private bool isFrighted;

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(AIComponent), typeof(MovementComponent));
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            if (isFrighted)
            {
                frightTime += delta;
                if (frightTime >= AIComponent.FRIGHTENED_TIME)
                {
                    UnFrightenGhosts();
                }
                else if (frightTime >= AIComponent.FRIGHTENED_TIME / 3) //end fright
                {
                    // todo
                }
            }
        }

        public override void ProcessEntity(GameEntity entity, float delta)
        {
            AIComponent ai = entity.GetComponent<AIComponent>();
            ai.controller.Update(delta);
        }

        public void FrightenGhosts()
        {
            engine.GetSystem<GridRenderSystem>().SetColor(Color.Red);
            foreach (var ghost in entities)
            {
                AIComponent ai = ghost.GetComponent<AIComponent>();
                ai.controller.SetState(AI.AIController.State.FRIGHTENED);
            }
            frightTime = 0;
            isFrighted = true;
        }

        public void UnFrightenGhosts()
        {
            engine.GetSystem<GridRenderSystem>().SetColor(Color.White);
            foreach (var ghost in entities)
            {
                AIComponent ai = ghost.GetComponent<AIComponent>();
                if (ai.controller.GetState() != AI.AIController.State.DEAD)
                    ai.controller.SetState(AI.AIController.State.SCATTER);
            }
            isFrighted = false;
        }

        public void RespawnGhosts()
        {
            foreach (var ghost in entities)
            {
                AIComponent ai = ghost.GetComponent<AIComponent>();
                ai.controller.Respawn();
            }
        }

        public bool IsFrighted()
        {
            return isFrighted;
        }
    }
}
