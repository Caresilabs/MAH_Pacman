using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using MAH_Pacman.Scene2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity
{
    public class Tile
    {
        public enum TileType
        {
            PASSABLE, BLOCKED, GHOSTONLY
        }

        private Point[] walls;
        private TileType type;
        private LevelIO.MAP_TILES mapType;

        private bool hasPellet;

        public Tile(int type, bool defaultPellet = true)
        {
            this.SetType((LevelIO.MAP_TILES)type);
            this.hasPellet = this.type == TileType.PASSABLE ? (defaultPellet ? true : false) : false;
        }

        public void Draw(SpriteBatch batch, int x, int y, SpriteComponent sprite, Color gridColor, int wallThickness)
        {
            // Background tile
            if (type == TileType.GHOSTONLY)
                batch.Draw(Assets.GetRegion("tileGhost"), new Rectangle(
                        (int)((x * TileComponent.TILE_SIZE)), (int)(y * TileComponent.TILE_SIZE)
                       , (int)TileComponent.TILE_SIZE, (int)TileComponent.TILE_SIZE)
                        , Assets.GetRegion("tileGhost"), Color.White, 0, Vector2.Zero, SpriteEffects.None, .1f); //SlateBlue

            if (type == TileType.BLOCKED)
                batch.Draw(Assets.GetRegion("tileBlocked"), new Rectangle(
                        (int)((x * TileComponent.TILE_SIZE)), (int)(y * TileComponent.TILE_SIZE)
                       , (int)TileComponent.TILE_SIZE, (int)TileComponent.TILE_SIZE)
                        , Assets.GetRegion("tileBlocked"), gridColor, 0, Vector2.Zero, SpriteEffects.None, .1f);


            if (HasWallWhere(World.DIRECTION_DOWN))
            {
                batch.Draw(sprite.texture, new Rectangle(
                    (int)(x * TileComponent.TILE_SIZE), (int)((y * TileComponent.TILE_SIZE) + TileComponent.TILE_SIZE - wallThickness)
                    , (int)TileComponent.TILE_SIZE, wallThickness)
                    , sprite.source, gridColor);
            }

            if (HasWallWhere(World.DIRECTION_UP))
            {
                batch.Draw(sprite.texture, new Rectangle(
                    (int)(x * TileComponent.TILE_SIZE), (int)(y * TileComponent.TILE_SIZE)
                    , (int)TileComponent.TILE_SIZE, wallThickness)
                    , sprite.source, gridColor);
            }

            if (HasWallWhere(World.DIRECTION_LEFT))
            {
                batch.Draw(sprite.texture, new Rectangle(
                    (int)(x * TileComponent.TILE_SIZE), (int)(y * TileComponent.TILE_SIZE)
                    , wallThickness, (int)TileComponent.TILE_SIZE)
                    , sprite.source, gridColor);
            }

            if (HasWallWhere(World.DIRECTION_RIGHT))
            {
                batch.Draw(sprite.texture, new Rectangle(
                    (int)(x * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE - wallThickness), (int)(y * TileComponent.TILE_SIZE)
                    , wallThickness, (int)TileComponent.TILE_SIZE)
                    , sprite.source, gridColor);
            }

            // pellet
            if (HasPellet())
            {
                batch.Draw(sprite.texture, new Rectangle(
                   (int)(x * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2 - 1), 
                   (int)(y * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2 - 1),
                   2, 2),
                   sprite.source, Color.Yellow);
            }
        }

        public void SetMapType(LevelIO.MAP_TILES type)
        {
            this.mapType = type;
        }

        public LevelIO.MAP_TILES GetMapType()
        {
            return mapType;
        }

        public void SetType(int type)
        {
            this.type = (TileType)type;
            this.walls = new Point[4] { 
                World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE
            };
        }

        public void SetType(LevelIO.MAP_TILES type)
        {
            if ((int)type <= 2)
                SetType((int)type);
            else
                SetType(0);

            SetMapType(type);
        }

        public TileType Type()
        {
            return type;
        }

        public bool Passable()
        {
            return (int)type != 1;
        }

        public bool TryEatPellet()
        {
            if (hasPellet)
            {
                hasPellet = false;
                return true;
            }
            return false;
        }

        public bool HasPellet()
        {
            if (hasPellet)
            {
                return true;
            }
            return false;
        }

        public void UpdateWalls(Point[] directions)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                walls[i] = directions[i];
            }
        }

        public bool HasWallWhere(Point direction)
        {
            return walls.Any(x => x == direction);
        }
    }
}
