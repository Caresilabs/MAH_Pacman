using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Entity.Systems;
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
        public static readonly Point DIRECTION_UP = new Point(0, -1);
        public static readonly Point DIRECTION_DOWN = new Point(0, 1);
        public static readonly Point DIRECTION_LEFT = new Point(-1, 0);
        public static readonly Point DIRECTION_RIGHT = new Point(1, 0);
        public static readonly Point DIRECTION_NONE = new Point(0, 0);

        public const int WIDTH = 14;
        public const int HEIGHT = 18;

        private Engine engine;

        private GameEntity pacman;

        public World(Engine engine)
        {
            this.engine = engine;
            this.initEntities();
        }

        private void initEntities()
        {
            CreateLevel(1);

            pacman = CreatePacman(0, 0);
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
                    grid.grid[i, j] = new Tile(inputMap[i, j]);
                }
            }

            var sprite = new SpriteComponent(Assets.getItems());
            sprite.source = Assets.getRegion("pixel");

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
            var sprite = new SpriteComponent(Assets.getItems());
            var animation = new AnimationComponent(0, 0, 16, 3, .2f);

            transform.position = new Vector2(5, 5);
            transform.size = new Vector2(1, 1);

            movement.velocity = new Vector2(0, 0);

            sprite.origin = new Vector2(8, 8);

            entity.Add(movement, transform, sprite, animation, pm);
            engine.Add(entity);

            return entity;
        }

        public void Update(float delta)
        {
           
        }
    }
}
