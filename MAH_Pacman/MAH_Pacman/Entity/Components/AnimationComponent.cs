using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class AnimationComponent : Component
    {
        private Dictionary<string, Animation> animations;
        private Animation current;

        public AnimationComponent()
        {
            this.animations = new Dictionary<string, Animation>();
        }

        public void Add(string name, Animation animation)
        {
            if (animations.Count == 0)
                current = animation;

            this.animations.Add(name, animation);
        }

        public void Set(string name)
        {
            this.current = animations[name];
        }

        public Animation GetCurrent()
        {
            return current;
        }
    }
}
