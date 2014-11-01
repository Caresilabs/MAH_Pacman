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
using MAH_Pacman.Tools;
using MAH_Pacman.Scene2D;

namespace MAH_Pacman.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class LevelEditorScreen : Screen, EventListener
    {
        private Engine engine;
        private Camera2D camera;
        private Scene scene;
        private Rectangle field;
        private LevelIO.MAP_TILES currentTile;

        private UIWindow selectTileWindow;

        private float stateTime;

        public override void Init()
        {
            this.currentTile = LevelIO.MAP_TILES.MAP_PASSABLE;
            this.engine = new Engine();
            this.InitLevel(1);
            this.camera = new Camera2D(GetGraphics(), 224, 288);
            this.scene = new Scene(camera, this);
            this.InitUI();
            this.field = new Rectangle(0, 0, (int)camera.GetWidth(), (int)(camera.GetHeight() - TileComponent.TILE_SIZE*2));
            this.InitSystems();
        }

        private void InitSystems()
        {
            // Logic Systems
            this.engine.Add(new PacmanSystem(null));
            this.engine.Add(new AISystem());
            this.engine.Add(new GridSystem(2));

            // Render Systems
            this.engine.Add(new GridRenderSystem(camera));
            this.engine.Add(new DrawSystem(camera));
        }

        private void InitUI()
        {
            UISpriteButton currentTileButton = new UISpriteButton(Assets.GetRegion("pacman"), 10, -TileComponent.TILE_SIZE, 24, 24);

            scene.Add("currentTileButton", currentTileButton);
        }

        private void InitLevel(int level)
        {
            GameEntity entity = new GameEntity();

            GridComponent grid = new GridComponent();
            grid.grid = new Tile[World.WIDTH, World.HEIGHT];

            int[,] inputMap = LevelIO.ReadLevel(level);

            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    int type = inputMap[i, j];

                    switch (type)
                    {
                        case (int)LevelIO.MAP_TILES.MAP_PACMAN:
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_BLINKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_INKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_PINKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_CLYDE:
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_ENERGIZER:
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_FRUIT:
                            break;
                        default:
                            break;
                    }
                    if (type <= 2 && type != 9)
                        grid.grid[i, j] = new Tile(type, false);
                    else
                        grid.grid[i, j] = new Tile((int)LevelIO.MAP_TILES.MAP_PASSABLE, false);
                }
            }

            var sprite = new SpriteComponent(Assets.GetRegion("pixel"));

            entity.Add(grid, sprite);
            engine.Add(entity);
        }

        public override void Update(float delta)
        {
            stateTime += delta;

            scene.Update(delta);
            engine.Update(delta);
            UpdateInput();
        }

        private void UpdateInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mouse = camera.Unproject(Mouse.GetState().X, Mouse.GetState().Y);

                if (field.Contains((int)mouse.X, (int)mouse.Y))
                {
                    //(int)(((x - position.X) / World.FIELD_WIDTH) * World.FIELD_SIZE);
                    int x = (int)(((mouse.X) / camera.GetWidth()) * World.WIDTH + .0f);
                    int y = (int)(((mouse.Y / camera.GetHeight()) * World.HEIGHT) + .0f);

                    engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().grid[x, y].SetType((int)currentTile);
                    engine.GetSystem<GridSystem>().UpdateAllWalls();
                }
            }

            if (InputHandler.KeyReleased(Keys.Space))
            {
                LevelIO.WriteLevel(engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().grid, 1);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);
            engine.Draw(batch);
            scene.Draw(batch);
        }

        public override void Dispose()
        {
        }

        public void EventCalled(Events e, Actor actor)
        {
            if (actor.name == "currentTileButton")
            {
                if (e == Events.TouchUp)
                {
                    UIWindow win = new UIWindow(200, 200);
                    scene.Add("win", win);
                }
            }
        }
    }
}
