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
        private TextureRegion[] corners;
        private TileType type;

        private bool hasPellet;

        public Tile(int type, bool defaultPellet = true)
        {
            this.SetType(type);
            this.hasPellet = this.type == TileType.PASSABLE ? (defaultPellet ? true : false) : false;
            this.walls = new Point[4] { 
                World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE
            };
            this.corners = new TextureRegion[4];
        }

        public void Draw(SpriteBatch batch, int x, int y, SpriteComponent sprite, Color gridColor, int wallThickness)
        {

            //if (corners[0] != null)
            //{
            //    TextureRegion region = corners[0];
            //    batch.Draw(region, new Rectangle(
            //        (int)(x * TileComponent.TILE_SIZE), (int)(y * TileComponent.TILE_SIZE)
            //       , (int)TileComponent.TILE_SIZE / 2, (int)TileComponent.TILE_SIZE / 2)
            //        , region, gridColor);
            //}

            //if (corners[1] != null)
            //{
            //    TextureRegion region = corners[1];
            //    batch.Draw(region, new Rectangle(
            //        (int)((x * TileComponent.TILE_SIZE) + TileComponent.TILE_SIZE / 2), (int)(y * TileComponent.TILE_SIZE)
            //       , (int)TileComponent.TILE_SIZE / 2, (int)TileComponent.TILE_SIZE / 2)
            //        , region, gridColor);
            //}

            //if (corners[2] != null)
            //{
            //    TextureRegion region = corners[2];
            //    batch.Draw(region, new Rectangle(
            //        (int)(x * TileComponent.TILE_SIZE), (int)(y * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2)
            //       , (int)TileComponent.TILE_SIZE / 2, (int)TileComponent.TILE_SIZE / 2)
            //        , region, gridColor);
            //}

            //if (corners[3] != null)
            //{
            //    TextureRegion region = corners[3];
            //    batch.Draw(region, new Rectangle(
            //        (int)((x * TileComponent.TILE_SIZE) + TileComponent.TILE_SIZE / 2), (int)(y * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2)
            //       , (int)TileComponent.TILE_SIZE / 2, (int)TileComponent.TILE_SIZE / 2)
            //        , region, gridColor);
            //}


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

        public void SetCorner(int corner, string type)
        {
            corners[corner] = Assets.GetRegion("tile" + type + corner);
        }

        public void SetType(int type)
        {
            this.type = (TileType)type;
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
