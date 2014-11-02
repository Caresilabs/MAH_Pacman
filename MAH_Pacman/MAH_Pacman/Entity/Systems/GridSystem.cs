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
        public Rectangle bounds;

        public GridSystem(int y)
        {
            this.bounds = new Rectangle(0, y, World.WIDTH, World.HEIGHT - y);
        }

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(GridComponent));
        }

        public override void Init()
        {
            UpdateAllWalls();
        }

        public override void Update(float delta)
        {
        }

        public int PelletsCount()
        {
            int pellets = 0;
            for (int j = 0; j < entities[0].GetComponent<GridComponent>().grid.GetLength(1); j++)
            {
                for (int i = 0; i < entities[0].GetComponent<GridComponent>().grid.GetLength(0); i++)
                {
                    Tile tile = entities[0].GetComponent<GridComponent>().grid[i, j];
                    if (tile.HasPellet())
                        pellets++;
                }
            }
            return pellets;
        }

        public bool IsForwardWalkable(GameEntity entity)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            return IsWalkable(entity, movement.velocity);
        }

        public bool IsWalkable(GameEntity entity, Point direction)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent position = entity.GetComponent<TransformationComponent>();

            int signVelocityX = Math.Sign(direction.X);
            int signVelocityY = Math.Sign(direction.Y);

            Tile tile = GetTileModulus(position.GetIntX() + signVelocityX, position.GetIntY() + signVelocityY);
            if (tile != null && IsPassable(tile.Type(),entity))
                return true;

            return false;
        }

        public bool IsWalkable(GameEntity entity, Vector2 direction)
        {
            return IsWalkable(entity, new Point((int)direction.X, (int)direction.Y));
        }

        private bool IsPassable(Tile.TileType type, GameEntity entity)
        {
            if (type == Tile.TileType.PASSABLE || (type == Tile.TileType.GHOSTONLY && entity.HasComponent<AIComponent>())) return true;
            else return false;
        }

        public bool CanTurn(GameEntity entity, Point direction)
        {
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent position = entity.GetComponent<TransformationComponent>();

            if (HasWalkedHalf(direction, position))
            {
                //Tile tile = GetTile(position.GetIntX() + direction.X, position.GetIntY() + direction.Y);
                ///if (tile != null && tile.Type() == Tile.TileType.PASSABLE) return true;
                return true;
            }
            return false;
        }

        public bool HasWalkedHalf(Point direction, TransformationComponent position)
        {
            //check if he over half the tile
            Vector2 normalPosition = new Vector2(position.position.X + .5f, position.position.Y + .5f) - new Vector2((int)(position.position.X + .5f) + .5f, (int)(position.position.Y + .5f) + .5f);

            if (!Vector2.Equals(normalPosition, Vector2.Zero))
                normalPosition.Normalize();


            Vector2 normalVelocity = new Vector2(direction.X, direction.Y);

            // Vertical movement
            normalPosition.X = Math.Sign(normalPosition.X);
            normalPosition.Y = Math.Sign(normalPosition.Y);

            if (Vector2.Equals(normalPosition, normalVelocity) || Vector2.Equals(Vector2.Zero, normalPosition))
            {
                return true;
            }
            return false;
        }

        public bool HasWalkedHalf(Vector2 direction, TransformationComponent position)
        {
            Vector2 dir = new Vector2(direction.X, direction.Y);
            dir.Normalize();
            return HasWalkedHalf(new Point((int)dir.X, (int)dir.Y), position);
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

        public void UpdateAllWalls()
        {
            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    UpdateTileWalls(i, j);
                }
            }
        }

        public void UpdateTileWalls(int x, int y)
        {
            Tile tile = GetTile(x, y);

            if (tile != null && !(tile.Type() == Tile.TileType.PASSABLE || tile.Type() == Tile.TileType.GHOSTONLY)) return;

            Point[] walls = new Point[4] { 
                World.DIRECTION_NONE, World.DIRECTION_NONE,World.DIRECTION_NONE,World.DIRECTION_NONE
            };

            Tile tempTile;

            tempTile = GetTileModulus(x, y - 1);
            if (tempTile != null && tempTile.Type() == Tile.TileType.BLOCKED)
                walls[0] = World.DIRECTION_UP;

            tempTile = GetTileModulus(x, y + 1);
            if (tempTile != null && tempTile.Type() == Tile.TileType.BLOCKED)
                walls[1] = World.DIRECTION_DOWN;

            tempTile = GetTileModulus(x - 1, y);
            if (tempTile != null && tempTile.Type() == Tile.TileType.BLOCKED)
                walls[2] = World.DIRECTION_LEFT;

            tempTile = GetTileModulus(x + 1, y);
            if (tempTile != null && tempTile.Type() == Tile.TileType.BLOCKED)
                walls[3] = World.DIRECTION_RIGHT;

            tile.UpdateWalls(walls);

            // Check topleft
            UpdateCorner(new Point(-1, -1), tile, x, y, 0);
            UpdateCorner(new Point(1, -1), tile, x, y, 1);
            UpdateCorner(new Point(-1, 1), tile, x, y, 2);
            UpdateCorner(new Point(1, 1), tile, x, y, 3);
        }

        private void UpdateCorner(Point direction, Tile tile, int x, int y, int id)
        {
            Tile[] tiles = GetCornerNeighbours(direction, x, y);

            if (tiles[0].Passable() && tiles[2].Passable()) tile.SetCorner(id, "Out"); // Corner
            if (!tiles[0].Passable() && tiles[1].Passable() && tiles[2].Passable()) tile.SetCorner(id, "Wall"); // horizontal wall
            if (tiles[0].Passable() && tiles[1].Passable() && !tiles[2].Passable()) tile.SetCorner(id, "Wall"); // vertical wall

            if (!tiles[0].Passable() && tiles[1].Passable() && !tiles[2].Passable()) tile.SetCorner(id, "In"); // inner

            //if (!tiles[0].Passable() && !tiles[1].Passable() && tiles[2].Passable()) tile.SetCorner(id, "In"); // inner horixontal
            if (tiles[0].Passable() && !tiles[1].Passable() && !tiles[2].Passable()) tile.SetCorner(id, "In"); // inner vertical
        }

        private Tile[] GetCornerNeighbours(Point direction, int x, int y) {
            Tile[] tiles = new Tile[3];

            tiles[0] = GetTileModulus(x + direction.X, y);
            tiles[1] = GetTileModulus(x + direction.X, y + direction.Y);
            tiles[2] = GetTileModulus(x, y + direction.Y);

            return tiles;
        }

        public void RemovePacman()
        {
            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    Tile tile = entities[0].GetComponent<GridComponent>().grid[i, j];
                    if (tile.GetMapType() == LevelIO.MAP_TILES.MAP_PACMAN)
                        tile.SetType(LevelIO.MAP_TILES.MAP_PASSABLE);
                }
            }
        }
    }
}
