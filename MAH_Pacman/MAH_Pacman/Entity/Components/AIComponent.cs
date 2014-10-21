using MAH_Pacman.AI;
using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Components
{
    public class AIComponent : Component
    {
        public static float SIZE = 1;
        
        public float speed = 1.9f;

        public AIController controller;

    }
}
