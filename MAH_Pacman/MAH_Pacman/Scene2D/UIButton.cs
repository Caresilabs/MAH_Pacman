using MAH_Pacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Scene2D
{
    public class UIButton : Actor
    {
        private Rectangle bounds;
        private Vector2 startPos;
        private Color color;

        private string text;
        private float scale;
        private float textWidth;

        public UIButton(string text, float x, float y, float scale = 1)
            : base(x, y, 10, 10)
        {
            this.text = text;
            this.scale = scale;
            this.color = Color.White;
            this.startPos = new Vector2(x, y);
            this.bounds = new Rectangle((int)x - (int)(Assets.font.MeasureString(text).Length() / 2 * (scale * UIConfig.DEFAULT_FONT_SCALE)), (int)y,
                (int)(Assets.font.MeasureString(text).Length() * (scale * UIConfig.DEFAULT_FONT_SCALE)), (int)(Assets.font.MeasureString(text).Y * (scale * UIConfig.DEFAULT_FONT_SCALE)));
            this.textWidth = (int)(Assets.font.MeasureString(text).Length() * (scale * UIConfig.DEFAULT_FONT_SCALE));

            SetRegion(Assets.GetRegion("button"));
            SetPosition(bounds.X - bounds.Width / 20, bounds.Y - bounds.Width / 10);
            SetSize(bounds.Width + bounds.Width / 10, bounds.Height + bounds.Width / 20);
        }

        public override void TouchDown(Vector2 mouse)
        {
            base.TouchDown(mouse);
            color = Color.Green;
        }

        public override void TouchUp(Vector2 mouse)
        {
            base.TouchUp(mouse);
            color = Color.White;
        }

        public override void TouchLeave(Vector2 mouse)
        {
            base.TouchLeave(mouse);
            color = Color.White;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(GetRegion(), GetBounds(), GetRegion(), color, 0, Vector2.Zero, SpriteEffects.None, .1f);
            batch.DrawString(Assets.font, text, new Vector2(GetX() + (GetBounds().Width - textWidth)*1.5f, GetY()), Color.White, 0, Vector2.Zero, scale * UIConfig.DEFAULT_FONT_SCALE, SpriteEffects.None, 0);

            base.Draw(batch);
        }

        public void SetText(string text)
        {
            this.text = text;
            this.bounds = new Rectangle((int)startPos.X - (int)(Assets.font.MeasureString(text).Length() / 2 * scale * UIConfig.DEFAULT_FONT_SCALE),
                (int)startPos.Y, (int)(Assets.font.MeasureString(text).Length() * scale * UIConfig.DEFAULT_FONT_SCALE), (int)Assets.font.MeasureString(text).Y);

            SetPosition(bounds.X - 20, bounds.Y - 10);
            SetSize(bounds.Width + 30, bounds.Height + 30);
        }
    }
}
