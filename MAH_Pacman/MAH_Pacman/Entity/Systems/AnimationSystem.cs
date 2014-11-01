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

            animation.GetCurrent().stateTime += delta;
            int currentFrame = (int)(animation.GetCurrent().stateTime / (float)animation.GetCurrent().frameDuration) % animation.GetCurrent().frames;

            sprite.source = new Rectangle(
                animation.GetCurrent().origin.X + (animation.GetCurrent().size*currentFrame), animation.GetCurrent().origin.Y, 
                    animation.GetCurrent().size, animation.GetCurrent().size);

        }
    }
}
