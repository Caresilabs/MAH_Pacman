using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class AnimationSystem : IteratingSystem
    {
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(AnimationComponent), typeof(SpriteComponent));
        }

        public override void ProcessEntity(GameEntity entity, float delta)
        {
            AnimationComponent animation = entity.GetComponent<AnimationComponent>();
            SpriteComponent sprite = entity.GetComponent<SpriteComponent>();

            animation.stateTime += delta;
            int currentFrame = (int)(animation.stateTime / (float)animation.frameDuration) % animation.frames;

            sprite.source = new Rectangle(
                animation.origin.X + (animation.size*currentFrame), animation.origin.Y, 
                    animation.size, animation.size);

        }
    }
}
