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
using MAH_Pacman.Scene2D;
using MAH_Pacman.Tools;

namespace MAH_Pacman.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class GameScreen : Screen
    {

        private World world;
        private Engine engine;
        private HUD hud;
        private Camera2D camera;

        private float stateTime;

        public GameScreen(int level = 1)
        {
            this.engine = new Engine();
            this.world = new World(engine, level);
            this.hud = new HUD(this);
        }

        public override void Init()
        {
            this.camera = new Camera2D(GetGraphics(), 224, 288);
            this.InitSystems();
        }

        private void InitSystems()
        {
            // Logic Systems
            this.engine.Add(new PacmanSystem(world));
            this.engine.Add(new AISystem());
            this.engine.Add(new GridSystem(2));
            this.engine.Add(new AnimationSystem());
            this.engine.Add(new MovementSystem());
            this.engine.Add(new CollisionSystem(world));

            // Render Systems
            this.engine.Add(new GridRenderSystem(camera));
            this.engine.Add(new DrawSystem(camera));

            engine.Update(0);
        }

        public override void Update(float delta)
        {
            stateTime += delta;

            if (world.GetState() != World.GameState.BEGIN)
                engine.Update(delta);
            
            hud.Update(delta);

            if (world.Update(delta))
            {
                stateTime = 0;
            }

            switch (world.GetState())
            {
                case World.GameState.PAUSED:
                    break;
                case World.GameState.RUNNING:
                    break;
                case World.GameState.BEGIN:
                    break;
                case World.GameState.GAMEOVER:
                    if (InputHandler.Clicked())
                    {
                        SetScreen(new MainMenuScreen());
                    }
                    break;
                case World.GameState.WIN:
                    if (world.GetLevel() == LevelIO.LEVEL_MAX)
                    {
                        SetScreen(new MainMenuScreen());
                    }
                    else
                    {
                        if (stateTime > 5)
                        {
                            SetScreen(new GameScreen(world.GetLevel() + 1));
                        }
                    }
                    break;
                default:
                    break;
            }

            if (InputHandler.KeyReleased(Keys.M))
                SetScreen(new MainMenuScreen());
            if (InputHandler.KeyReleased(Keys.R))
                SetScreen(new GameScreen());
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);
            engine.Draw(batch);
            hud.Draw(batch);
        }

        public override void Dispose()
        {
        }

        public World GetWorld()
        {
            return world;
        }

        public Camera2D GetCamera()
        {
            return camera;
        }
    }
}
