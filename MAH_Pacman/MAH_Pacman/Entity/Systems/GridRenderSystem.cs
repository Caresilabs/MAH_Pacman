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
        public Camera2D camera;

        private List<ScoreEffect> scoreEffects;
        private Color gridColor;
        private Color blinkColor;

        private int wallThickness;
        private bool isBlinking;
        private float blinkTime;

        public override Type[] RequeredComponents()
        {
            return FamilyFor(typeof(GridComponent), typeof(SpriteComponent));
        }

        public GridRenderSystem(Camera2D camera)
        {
            this.scoreEffects = new List<ScoreEffect>();
            this.camera = camera;
            this.camera.SetPosition(0, -TileComponent.TILE_SIZE * 2);
            this.wallThickness = (int)(TileComponent.TILE_SIZE / 10);
            this.isBlinking = false;
            this.blinkTime = 0;
            this.gridColor = Color.White;
        }

        public void StartBlinking(Color? blinkColor = null)
        {
            this.blinkTime = 0;
            this.isBlinking = true;

            if (blinkColor == null)
                this.blinkColor = Color.Green;
            else
                this.blinkColor = (Color)blinkColor;
        }

        public void StopBlinking()
        {
            this.isBlinking = false;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            UpdateBlinking(delta);
            UpdateScoreEffects(delta);
        }

        private void UpdateScoreEffects(float delta)
        {
            for (int i = 0; i < scoreEffects.Count; i++)
            {
                ScoreEffect score = scoreEffects[i];
                score.Update(delta);
                if (! score.isAlive())
                {
                    scoreEffects.Remove(score);
                }
            }
        }

        private void UpdateBlinking(float delta)
        {
            if (isBlinking)
            {
                blinkTime += delta;

                if (blinkTime > .2f)
                {
                    gridColor = blinkColor;
                }
                if (blinkTime > .4f)
                {
                    gridColor = Color.White;
                    blinkTime = 0;
                }
            }
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
                        tile.Draw(batch, i, j, sprite, gridColor, wallThickness);
                    }
                }
            }

            // Draw effects
            for (int i = 0; i < scoreEffects.Count; i++)
            {
                ScoreEffect score = scoreEffects[i];
                score.Draw(batch);
            }

            batch.End();
        }

        public void AddScoreEffect(float x, float y, int score)
        {
            ScoreEffect sc = new ScoreEffect(x, y, score);
            scoreEffects.Add(sc);
        }

        public void SetColor(Color color)
        {
            this.gridColor = color;
        }
    }
}
