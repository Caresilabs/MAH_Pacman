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
    public class GameScreen : Screen
    {

        private World world;
        private Engine engine;

        private float stateTime;

        public GameScreen()
        {
        }

        public override void Init()
        {
            TileComponent.TILE_SIZE = 224 / (float)World.WIDTH;

            this.engine = new Engine();
            this.world = new World(engine);
            this.InitSystems();
        }

        private void InitSystems()
        {
            this.engine.Add(new PacmanSystem());
            this.engine.Add(new GridSystem(2));
            this.engine.Add(new AnimationSystem());
            this.engine.Add(new MovementSystem());

            this.engine.Add(new DrawSystem(GetGraphics()));
            this.engine.Add(new GridRenderSystem(engine.GetSystem<DrawSystem>().camera));
        }

        public override void Update(float delta)
        {
            stateTime += delta;

            world.Update(delta);
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
