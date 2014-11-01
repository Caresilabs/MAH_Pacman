using MAH_Pacman.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity
{
    public class ScoreEffect
    {
        public const float MAX_ALIVE_TIME = 1;

        private Vector2 position;

        private float stateTime;
        private int score;
        private bool alive;

        public ScoreEffect(float x, float y, int score)
        {
            this.position = new Vector2(x * TileComponent.TILE_SIZE + 4, y * TileComponent.TILE_SIZE + 3);
            this.alive = true;
            this.score = score;
        }

        public void Update(float delta)
        {
            stateTime += delta;
            if (stateTime >= MAX_ALIVE_TIME)
                alive = false;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(Assets.font, score.ToString(), position, Color.White, 0, Vector2.Zero, .2f, SpriteEffects.None, 0);
        }

        public bool isAlive()
        {
            return alive;
        }
    }
}
