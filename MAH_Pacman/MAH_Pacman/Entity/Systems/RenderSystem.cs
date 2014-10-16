using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity.Systems
{
    public class RenderSystem : EntitySystem
    {
        public override Type[] RequeredComponents()
        {
            return Requered(typeof(PositionComponent), typeof(SpriteComponent));
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (var item in Entities)
            {
                PositionComponent position = item.GetComponent<PositionComponent>();
                SpriteComponent sprite = item.GetComponent<SpriteComponent>();

                position.Position+= new Vector2(2, 1);
                batch.Draw(sprite.Texture, position.Position, Color.White);

            }
        }
    }
}
