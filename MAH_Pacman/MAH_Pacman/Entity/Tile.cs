using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
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

        private bool hasPellet;

        public Tile(int type)
        {
            this.SetType(type);
            this.hasPellet = this.type == TileType.PASSABLE ? true : false;
            this.walls = new Point[4] { 
                World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE, World.DIRECTION_NONE
            };
        }

        public void SetType(int type)
        {
            this.type = (TileType)type;
        }

        public TileType Type()
        {
            return type;
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
