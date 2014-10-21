using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MAH_Pacman.Model;
using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Systems;
using MAH_Pacman.Entity.Components;

namespace MAH_Pacman.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class LevelEditorScreen : Screen
    {

        private Engine engine;

        private float stateTime;

        public override void Init()
        {
            this.engine = new Engine();
            this.InitSystems();
        }

        private void InitSystems()
        {
            // Logic Systems
            this.engine.Add(new PacmanSystem());
            this.engine.Add(new AISystem());
            this.engine.Add(new GridSystem(2));

            // Render Systems
            this.engine.Add(new GridRenderSystem(GetGraphics()));
            this.engine.Add(new DrawSystem(engine.GetSystem<GridRenderSystem>().camera));
        }

        public override void Update(float delta)
        {
            stateTime += delta;

            engine.Update(delta);
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);
            engine.Draw(batch);
        }

        public override void Dispose()
        {
        }
    }
}
