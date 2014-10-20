using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using MAH_Pacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class GridRenderSystem : RenderSystem
    {
        private Camera2D camera;
        private int wallThickness;

        public GridRenderSystem(Camera2D camera)
        {
            this.camera = camera;
            this.wallThickness = (int)(TileComponent.TILE_SIZE / 10);
        }

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(GridComponent), typeof(SpriteComponent));
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       SamplerState.PointClamp,
                       null,
                       null,
                       null,
                       camera.Update().GetMatrix());

            foreach (var item in entities)
            {
                GridComponent grid = item.GetComponent<GridComponent>();
                SpriteComponent sprite = item.GetComponent<SpriteComponent>();

                for (int j = 0; j < grid.grid.GetLength(1); j++)
                {
                    for (int i = 0; i < grid.grid.GetLength(0); i++)
                    {
                        Tile tile = grid.grid[i, j];

                        //batch.Draw(sprite.Texture, new Rectangle((int)(i * TileComponent.TILE_SIZE), (int)(j * TileComponent.TILE_SIZE)                           , (int)TileComponent.TILE_SIZE, (int)TileComponent.TILE_SIZE), sprite.Source, Color.Gray, 0,Vector2.Zero, SpriteEffects.None, .5f);

                        if (tile.HasWallWhere(World.DIRECTION_DOWN))
                        {
                            batch.Draw(sprite.texture, new Rectangle(
                                (int)(i * TileComponent.TILE_SIZE), (int)((j * TileComponent.TILE_SIZE) + TileComponent.TILE_SIZE - wallThickness)
                                , (int)TileComponent.TILE_SIZE, wallThickness)
                                , sprite.source, Color.White);
                        }

                        if (tile.HasWallWhere(World.DIRECTION_UP))
                        {
                            batch.Draw(sprite.texture, new Rectangle(
                                (int)(i * TileComponent.TILE_SIZE), (int)(j * TileComponent.TILE_SIZE)
                                , (int)TileComponent.TILE_SIZE, wallThickness)
                                , sprite.source, Color.White);
                        }

                        if (tile.HasWallWhere(World.DIRECTION_LEFT))
                        {
                            batch.Draw(sprite.texture, new Rectangle(
                                (int)(i * TileComponent.TILE_SIZE), (int)(j * TileComponent.TILE_SIZE)
                                , wallThickness, (int)TileComponent.TILE_SIZE)
                                , sprite.source, Color.White);
                        }

                        if (tile.HasWallWhere(World.DIRECTION_RIGHT))
                        {
                            batch.Draw(sprite.texture, new Rectangle(
                                (int)(i * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE - wallThickness), (int)(j * TileComponent.TILE_SIZE)
                                , wallThickness, (int)TileComponent.TILE_SIZE)
                                , sprite.source, Color.White);
                        }

                        if (tile.HasPellet())
                        {
                            batch.Draw(sprite.texture, new Rectangle(
                               (int)(i * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2), (int)(j * TileComponent.TILE_SIZE + TileComponent.TILE_SIZE / 2)
                               , 2, 2)
                               , sprite.source, Color.Yellow);
                        }
                    }
                }
            }

            batch.End();
        }
    }
}
