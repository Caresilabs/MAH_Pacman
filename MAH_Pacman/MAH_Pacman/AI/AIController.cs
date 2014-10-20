﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public abstract class AIController
    {
        protected Point spawn;
        protected Point target;

        public AIController()
        {
            this.target = new Point();
            this.spawn = new Point();
        }

        public void Update(GameEntity ai, float delta)
        {

        }
    }
}
