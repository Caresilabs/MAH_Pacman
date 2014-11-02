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

        private int level;

        public override void Init()
        {
            this.currentTile = LevelIO.MAP_TILES.MAP_PASSABLE;
            this.engine = new Engine();
            this.level = level == 0 ? 1 : level;
            this.InitLevel(level);
            this.camera = new Camera2D(GetGraphics(), 224, 288);
            this.scene = new Scene(camera, this);
            this.InitUI();
            this.field = new Rectangle(0, 0, (int)camera.GetWidth(), (int)(camera.GetHeight() - TileComponent.TILE_SIZE * 2));
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
            UIButton currentTileButton = new UIButton("Play!", TileComponent.TILE_SIZE * 2, -TileComponent.TILE_SIZE * 1.5f, .7f);
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
                    grid.grid[i, j] = new Tile(type, false);
                }
            }

            var sprite = new SpriteComponent(Assets.GetRegion("pixel"));

            entity.Add(grid, sprite);
            engine.Add(entity);
        }

        public override void Update(float delta)
        {
            scene.Update(delta);
            engine.Update(delta);
            UpdateInput();
        }

        private void UpdateInput()
        {
            UpdateTileInput();

            if (InputHandler.KeyReleased(Keys.Space))
                SaveLevel();
            if (InputHandler.KeyReleased(Keys.M))
                SetScreen(new MainMenuScreen());
        }

        private void SaveLevel()
        {
            LevelIO.WriteLevel(engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().grid, level);
        }

        private void UpdateTileInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mouse = camera.Unproject(Mouse.GetState().X, Mouse.GetState().Y);

                if (field.Contains((int)mouse.X, (int)mouse.Y))
                {
                    //(int)(((x - position.X) / World.FIELD_WIDTH) * World.FIELD_SIZE);
                    int x = (int)(((mouse.X) / camera.GetWidth()) * World.WIDTH + .0f);
                    int y = (int)(((mouse.Y / (camera.GetHeight() + camera.GetPosition().Y)) * World.HEIGHT) + .0f);

                    if (currentTile == LevelIO.MAP_TILES.MAP_PACMAN)
                        engine.GetSystem<GridSystem>().RemovePacman();

                    engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().grid[x, y].SetType(currentTile);
                    engine.GetSystem<GridSystem>().UpdateAllWalls();
                }
            }

            // Change level
            if (InputHandler.KeyReleased(Keys.Left))
            {
                if (level > 1)
                    level -= 1; Init();
            }

            if (InputHandler.KeyReleased(Keys.Right))
            {
                if (level <= LevelIO.LEVEL_MAX)
                    level += 1; Init();
            }

            // Delete level
            if (InputHandler.KeyReleased(Keys.Delete))
            {
                if (level == LevelIO.LEVEL_MAX && level != 1)
                {
                    LevelIO.DeleteLastLevel();
                    level--;
                    Init();
                }
            }

            // Change tile
            if (InputHandler.KeyReleased(Keys.D1))
                currentTile = (LevelIO.MAP_TILES)0;
            if (InputHandler.KeyReleased(Keys.D2))
                currentTile = (LevelIO.MAP_TILES)1;
            if (InputHandler.KeyReleased(Keys.D3))
                currentTile = (LevelIO.MAP_TILES)2;
            if (InputHandler.KeyReleased(Keys.D4))
                currentTile = (LevelIO.MAP_TILES)3;
            if (InputHandler.KeyReleased(Keys.D5))
                currentTile = (LevelIO.MAP_TILES)4;
            if (InputHandler.KeyReleased(Keys.D6))
                currentTile = (LevelIO.MAP_TILES)5;
            if (InputHandler.KeyReleased(Keys.D7))
                currentTile = (LevelIO.MAP_TILES)6;
            if (InputHandler.KeyReleased(Keys.D8))
                currentTile = (LevelIO.MAP_TILES)7;
            if (InputHandler.KeyReleased(Keys.D9))
                currentTile = (LevelIO.MAP_TILES)8;
            if (InputHandler.KeyReleased(Keys.D0))
                currentTile = (LevelIO.MAP_TILES)9;
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);
            engine.Draw(batch);
            scene.Draw(batch);

            batch.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       SamplerState.LinearClamp,
                       null,
                       null,
                       null,
                       camera.Update().GetMatrix());

            DrawTiles(batch);

            HUD.DrawCenterString(batch, camera, "Level " + level, -TileComponent.TILE_SIZE * 1.5f, .35f);

            batch.End();

        }

        private void DrawTiles(SpriteBatch batch)
        {
            // Draw mouse tile
            Vector2 mouse = camera.Unproject(Mouse.GetState().X, Mouse.GetState().Y);
            if (field.Contains((int)mouse.X, (int)mouse.Y))
            {
                batch.Draw(GetRegionFor(currentTile), new Rectangle(
                        (int)(mouse.X - mouse.X % TileComponent.TILE_SIZE), (int)(mouse.Y - mouse.Y % TileComponent.TILE_SIZE)
                       , (int)TileComponent.TILE_SIZE, (int)TileComponent.TILE_SIZE)
                        , GetRegionFor(currentTile), Color.White, 0, Vector2.Zero, SpriteEffects.None, .1f);
            }

            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    Tile t = engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().grid[i, j];

                    if (GetRegionFor(t.GetMapType(), true) != null)
                        batch.Draw(GetRegionFor(t.GetMapType()), new Rectangle(
                           (int)(i * TileComponent.TILE_SIZE), (int)(j * TileComponent.TILE_SIZE)
                          , (int)TileComponent.TILE_SIZE, (int)TileComponent.TILE_SIZE)
                           , GetRegionFor(t.GetMapType()), Color.White, 0, Vector2.Zero, SpriteEffects.None, .4f);
                }
            }
        }

        private TextureRegion GetRegionFor(LevelIO.MAP_TILES tile, bool specialsOnly = false)
        {
            switch (tile)
            {
                case LevelIO.MAP_TILES.MAP_PASSABLE:
                    return specialsOnly ? null : Assets.GetRegion("pixel");
                case LevelIO.MAP_TILES.MAP_BLOCKED:
                    return specialsOnly ? null : Assets.GetRegion("pixel");
                case LevelIO.MAP_TILES.MAP_GHOSTONLY:
                    return specialsOnly ? null : Assets.GetRegion("pixelGreen");
                case LevelIO.MAP_TILES.MAP_PACMAN:
                    return Assets.GetRegion("pacman");
                case LevelIO.MAP_TILES.MAP_GHOST_BLINKY:
                    return Assets.GetRegion("ghost_blinky");
                case LevelIO.MAP_TILES.MAP_GHOST_PINKY:
                    return Assets.GetRegion("ghost_pinky");
                case LevelIO.MAP_TILES.MAP_GHOST_INKY:
                    return Assets.GetRegion("ghost_inky");
                case LevelIO.MAP_TILES.MAP_GHOST_CLYDE:
                    return Assets.GetRegion("ghost_clyde");
                case LevelIO.MAP_TILES.MAP_ENERGIZER:
                    return Assets.GetRegion("energizer");
                case LevelIO.MAP_TILES.MAP_FRUIT:
                    return Assets.GetRegion("fruit1");
                default:
                    break;
            }
            return null;
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
                    SaveLevel();
                    SetScreen(new GameScreen(level));
                }
            }
        }
    }
}
