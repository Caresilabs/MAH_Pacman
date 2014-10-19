using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class GridSystem : EntitySystem
    {
        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(GridComponent));
        }

        public override void Init()
        {
            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    UpdateTileWalls(i, j);
                }
            }
        }

        public override void Update(float delta)
        {
        }

        public bool IsForwardWalkable(GameEntity entity)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent position = entity.GetComponent<TransformationComponent>();

            if (movement.velocity.X > 0)
            {
                Tile tile = GetTile(position.GetIntX() + 1, position.GetIntY());
                if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;
            }
            else if (movement.velocity.X < 0)
            {
                Tile tile = GetTile(position.GetIntX() - 1, position.GetIntY());
                if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;
            }
            else if (movement.velocity.Y > 0)
            {
                Tile tile = GetTile(position.GetIntX(), position.GetIntY() + 1);
                if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;
            }
            else if (movement.velocity.Y < 0)
            {
                Tile tile = GetTile(position.GetIntX(), position.GetIntY() - 1);
                if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;
            }
            return false;
        }

        public bool CanTurn(GameEntity entity, Point direction)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent position = entity.GetComponent<TransformationComponent>();


            Tile tile = GetTile(position.GetIntX() + direction.X, position.GetIntY() + direction.Y);
            if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;

            return false;
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return null;

            return entities[0].GetComponent<GridComponent>().grid[x, y];
        }

        public Tile GetTileModulus(int x, int y)
        {
            if (x < 0) x = World.WIDTH + x;
            if (y < 0) y = World.HEIGHT + y;
            return entities[0].GetComponent<GridComponent>().grid[x % World.WIDTH, y % World.HEIGHT];
        }

        public void UpdateTileWalls(int x, int y)
        {
            Tile tile = GetTile(x, y);

            if (tile != null && tile.Type() != Tile.TileType.BLOCKED) return;

            Point[] walls = new Point[4] { 
                World.DIRECTION_NONE, World.DIRECTION_NONE,World.DIRECTION_NONE,World.DIRECTION_NONE
            };

            Tile tempTile;

            tempTile = GetTileModulus(x, y - 1);
            if (tempTile != null && tempTile.Type() == Tile.TileType.PASSABLE)
                walls[0] = World.DIRECTION_UP;

            tempTile = GetTileModulus(x, y + 1);
            if (tempTile != null && tempTile.Type() == Tile.TileType.PASSABLE)
                walls[1] = World.DIRECTION_DOWN;

            tempTile = GetTileModulus(x - 1, y);
            if (tempTile != null && tempTile.Type() == Tile.TileType.PASSABLE)
                walls[2] = World.DIRECTION_LEFT;

            tempTile = GetTileModulus(x + 1, y);
            if (tempTile != null && tempTile.Type() == Tile.TileType.PASSABLE)
                walls[3] = World.DIRECTION_RIGHT;

            tile.UpdateWalls(walls);
        }
    }
}
