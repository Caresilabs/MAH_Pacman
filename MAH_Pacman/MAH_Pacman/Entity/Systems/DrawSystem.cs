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
    public class DrawSystem : RenderSystem
    {
        public Camera2D camera;

        public DrawSystem(GraphicsDevice device)
        {
            this.camera = new Camera2D(device, 224 , 288);
        }

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(TransformationComponent), typeof(SpriteComponent));
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       SamplerState.LinearClamp,
                       null,
                       null,
                       null,
                       camera.Update().GetMatrix());

            foreach (var item in entities)
            {
                TransformationComponent position = item.GetComponent<TransformationComponent>();
                SpriteComponent sprite = item.GetComponent<SpriteComponent>();

                sprite.bounds.X = (int)(position.position.X * TileComponent.TILE_SIZE + sprite.origin.X);
                sprite.bounds.Y = (int)(position.position.Y * TileComponent.TILE_SIZE + sprite.origin.Y);
                sprite.bounds.Width = (int)(position.size.X * TileComponent.TILE_SIZE);
                sprite.bounds.Height = (int)(position.size.Y * TileComponent.TILE_SIZE);

                batch.Draw(sprite.texture, sprite.bounds, sprite.source, Color.White, (float)sprite.rotation, sprite.origin, SpriteEffects.None, 0);

            }
            batch.End();
        }
    }
}
