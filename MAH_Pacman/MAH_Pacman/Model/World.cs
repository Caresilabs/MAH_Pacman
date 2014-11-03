using MAH_Pacman.AI;
using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Entity.Systems;
using MAH_Pacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Model
{
    public class World
    {
        public enum GameState
        {
            PAUSED, RUNNING, BEGIN, GAMEOVER, WIN
        }

        public static readonly Point DIRECTION_UP = new Point(0, -1);
        public static readonly Point DIRECTION_DOWN = new Point(0, 1);
        public static readonly Point DIRECTION_LEFT = new Point(-1, 0);
        public static readonly Point DIRECTION_RIGHT = new Point(1, 0);
        public static readonly Point DIRECTION_NONE = new Point(0, 0);

        public const int WIDTH = 14;
        public const int HEIGHT = 16;

        public static GameEntity pacman;

        private Engine engine;
        private GameState state;
        private Point pacmanSpawn;

        private int level;
        private int lives;
        private int score;
        private bool newState;
        private float stateTime;
 
        public World(Engine engine, int level, int score = 0)
        {
            this.engine = engine;
            this.level = level;
            this.InitWorld(level);
            this.lives = 3;
            this.stateTime = 0;
            this.score = score;
            this.newState = false;
        }

        public void InitWorld(int level)
        {
            SetState(GameState.BEGIN);
            this.initEntities(level);
        }

        public bool Update(float delta)
        {
            stateTime += delta;
            if (state == GameState.BEGIN)
            {
                if (stateTime > 2.0f)
                    SetState(GameState.RUNNING);
            }
            if (newState)
            {
                newState = false;
                return true;
            }
            return false;
        }

        public void OnCollision(GameEntity e1, GameEntity e2)
        {
            // Collide against AI
            if (e1.HasComponent<AIComponent>() && e2.HasComponent<PacmanComponent>())
            {
                if (e1.GetComponent<AIComponent>().controller.CollideWithPacman())
                {
                    lives--;
                    if (lives <= 0)
                        SetState(GameState.GAMEOVER);
                    else
                        RespawnPacman();
                }
                else
                {
                    AddScore(AIComponent.SCORE, e2.GetComponent<TransformationComponent>());
                }
            }
           

            // Energizer
            if (e1.HasComponent<EnergizerComponent>() && e2.HasComponent<PacmanComponent>())
            {
                AddScore(EnergizerComponent.SCORE, e2.GetComponent<TransformationComponent>());
                engine.GetSystem<AISystem>().FrightenGhosts();
                engine.Remove(e1);
            }

            // Fruit
            if (e1.HasComponent<FruitComponent>() && e2.HasComponent<PacmanComponent>())
            {
                AddScore(FruitComponent.SCORE, e2.GetComponent<TransformationComponent>());
                engine.Remove(e1);
            }
        }

        private void RespawnPacman()
        {
            // Reset pacmans data
            pacman.GetComponent<MovementComponent>().velocity = new Vector2();
            pacman.GetComponent<MovementComponent>().halt = false;
            pacman.GetComponent<PacmanComponent>().direction = World.DIRECTION_NONE;
            pacman.GetComponent<PacmanComponent>().nextDirection = World.DIRECTION_NONE;
            pacman.GetComponent<TransformationComponent>().position = new Vector2(pacmanSpawn.X, pacmanSpawn.Y);

            // Respawn ghosts
            engine.GetSystem<AISystem>().RespawnGhosts();

            SetState(GameState.BEGIN);
        }

        private void initEntities(int lvl = 1)
        {
            CreateLevel(lvl);
        }

        private GameEntity CreateLevel(int level)
        {
            GameEntity entity = new GameEntity();

            GridComponent grid = new GridComponent();
            grid.grid = new Tile[WIDTH, HEIGHT];

            int[,] inputMap = LevelIO.ReadLevel(level);

            for (int j = 0; j < HEIGHT; j++)
            {
                for (int i = 0; i < WIDTH; i++)
                {
                    int type = inputMap[i, j];

                    switch (type)
                    {
                        case (int)LevelIO.MAP_TILES.MAP_PACMAN:
                            pacman = CreatePacman(i, j);
                            pacmanSpawn = new Point(i, j);
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_BLINKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_INKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_PINKY:
                        case (int)LevelIO.MAP_TILES.MAP_GHOST_CLYDE:
                            CreateGhost(i, j, (LevelIO.MAP_TILES)type);
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_ENERGIZER:
                            CreateEnergizer(i, j);
                            break;
                        case (int)LevelIO.MAP_TILES.MAP_FRUIT:
                            CreateFruit(i, j);
                            break;
                        default:
                            break;
                    }
                    if (type <= 2 && type != 9)
                    {
                        grid.grid[i, j] = new Tile(type);
                    }
                    else
                    {
                        grid.grid[i, j] = new Tile((int)LevelIO.MAP_TILES.MAP_PASSABLE, false);
                    }
                }
            }

            var sprite = new SpriteComponent(Assets.GetRegion("pixel"));

            entity.Add(grid, sprite);
            engine.Add(entity);

            return entity;
        }

        public GameEntity CreatePacman(float x, float y)
        {
            GameEntity entity = new GameEntity();

            var pm = new PacmanComponent();
            var movement = new MovementComponent();
            var transform = new TransformationComponent();
            var sprite = new SpriteComponent(Assets.items);
            var animation = new AnimationComponent();

            transform.position = new Vector2(x, y);
            transform.size = new Vector2(1, 1);
            movement.velocity = new Vector2(0, 0);
            sprite.origin = new Vector2(8, 8);
            animation.Add("walk", new Animation(0, 0, 16, 3, .15f));

            entity.Add(movement, transform, sprite, animation, pm);
            engine.Add(entity);

            return entity;
        }

        public GameEntity CreateGhost(float x, float y, LevelIO.MAP_TILES ghost)
        {
            GameEntity entity = new GameEntity();

            var ai = new AIComponent();
            var movement = new MovementComponent();
            var transform = new TransformationComponent();
            var sprite = new SpriteComponent(Assets.items);
            var animation = new AnimationComponent();

            transform.position = new Vector2(x, y);
            transform.size = new Vector2(1, 1);
            movement.velocity = new Vector2(1, 0);
            sprite.origin = new Vector2(8, 8);

            switch (ghost)
            {
                case LevelIO.MAP_TILES.MAP_GHOST_BLINKY:
                    ai.controller = new AIBlinky(entity, (int)x, (int)y);
                    break;
                case LevelIO.MAP_TILES.MAP_GHOST_PINKY:
                    ai.controller = new AIPinky(entity, (int)x, (int)y);
                    break;
                case LevelIO.MAP_TILES.MAP_GHOST_INKY:
                    ai.controller = new AIInky(entity, (int)x, (int)y);
                    break;
                case LevelIO.MAP_TILES.MAP_GHOST_CLYDE:
                    ai.controller = new AIClyde(entity, (int)x, (int)y);
                    break;
                default:
                    break;
            }
            
            animation.Add("walk", new Animation(0, (int)(ghost - 3) * 16, 16, 8, .2f));
            animation.Add("dead", new Animation(64, 80, 16, 4, .2f));
            animation.Add("frightened", new Animation(0, 80, 16, 2, .2f));
            animation.Add("frightenedEnd", new Animation(64, 80, 16, 2, .2f));

            entity.Add(movement, transform, sprite, animation, ai);
            engine.Add(entity);

            return entity;
        }

        public GameEntity CreateEnergizer(float x, float y)
        {
            GameEntity entity = new GameEntity();

            var transform = new TransformationComponent();
            var sprite = new SpriteComponent(Assets.GetRegion("energizer"));
            var energizer = new EnergizerComponent();
            var animation = new AnimationComponent();

            animation.Add("idle", new Animation(64, 0, 16, 2, .2f));

            transform.position = new Vector2(x, y);
            transform.size = new Vector2(1, 1);
            sprite.origin = new Vector2(1, 1);

            entity.Add(transform, sprite, energizer, animation);
            engine.Add(entity);

            return entity;
        }

        public GameEntity CreateFruit(float x, float y)
        {
            GameEntity entity = new GameEntity();

            var transform = new TransformationComponent();
            var sprite = new SpriteComponent(Assets.GetRegion("fruit" + (MathUtils.random(4) + 1)));
            var fruit = new FruitComponent();

            transform.position = new Vector2(x, y);
            transform.size = new Vector2(1, 1);
            sprite.origin = new Vector2(1, 1);

            entity.Add(transform, sprite, fruit);
            engine.Add(entity);

            return entity;
        }

        public void SetState(GameState state)
        {
            this.stateTime = 0;
            this.state = state;
            this.newState = true;

            switch (state)
            {
                case GameState.PAUSED:
                    break;
                case GameState.RUNNING:
                    break;
                case GameState.BEGIN:
                    if (Assets.SOUND)
                        Assets.introSound.Play();
                    break;
                case GameState.GAMEOVER:
                    HighscoreManager.SaveHighscore(GetScore());
                     engine.GetSystem<GridRenderSystem>().StartBlinking(Color.Red);
                    engine.Remove<MovementSystem>();
                    engine.Remove<CollisionSystem>();
                    engine.Remove<AnimationSystem>();

                    if (Assets.SOUND)
                        Assets.deathSound.Play();

                    break;
                case GameState.WIN:
                    HighscoreManager.SaveHighscore(GetScore());
                    engine.GetSystem<GridRenderSystem>().StartBlinking();
                    engine.Remove<MovementSystem>();
                    engine.Remove<CollisionSystem>();
                    engine.Remove<AnimationSystem>();
                    break;
                default:
                    break;
            }
        }

        public GameState GetState()
        {
            return state;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int score, float x, float y)
        {
            engine.GetSystem<GridRenderSystem>().AddScoreEffect(x, y, score);
            this.score += score;
        }

        private void AddScore(int score, TransformationComponent transform)
        {
            AddScore(score, transform.GetIntX(), transform.GetIntY());
        }

        public void AddScore(int score, Vector2 position)
        {
            AddScore(score, position.X, position.Y);
        }

        public int GetLives()
        {
            return lives;
        }
    }
}
