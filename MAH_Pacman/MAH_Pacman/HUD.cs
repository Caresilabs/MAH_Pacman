using MAH_Pacman.Controller;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman
{
    public class HUD
    {
        private World world;
        private GameScreen screen;

        public HUD(GameScreen screen)
        {
            this.screen = screen;
            this.world = screen.GetWorld();
        }

        public void Update(float delta)
        {

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     null,
                     null,
                     null,
                     screen.GetCamera().Update().GetMatrix());
           
            // Render lives
            for (int i = 0; i < world.GetLives(); i++)
            {
                batch.Draw(Assets.GetRegion("pacman"), new Rectangle(
                               5 + (int)(i * (int)(TileComponent.TILE_SIZE / 1.1f)), -(int)(TileComponent.TILE_SIZE * 1.5f)
                               , (int)(TileComponent.TILE_SIZE/1.3f), (int)(TileComponent.TILE_SIZE/1.3f)), Assets.GetRegion("pacman"),
                                 Color.White);
            }

            DrawStates(batch);

            batch.End();
        }

        private void DrawStates(SpriteBatch batch)
        {
            switch (world.GetState())
            {
                case World.GameState.PAUSED:
                    DrawCenterString(batch, "PAUSED!", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2, .4f);
                    break;
                case World.GameState.RUNNING:
                    DrawCenterString(batch, "Score: " + world.GetScore(), -TileComponent.TILE_SIZE * 2, .3f);
                    break;
                case World.GameState.BEGIN:
                    DrawCenterString(batch, "READY..!", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2, .4f);
                    break;
                case World.GameState.GAMEOVER:
                    DrawCenterString(batch, "GAMEOVER!", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2 - 27, .5f);
                    DrawCenterString(batch, "Score: " + world.GetScore(), screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2, .4f);

                    DrawCenterString(batch, "Click to continue...", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2 + 20, .25f);
                    break;
                case World.GameState.WIN:
                    DrawCenterString(batch, "GRATZ!!", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2 - 20,  Color.Magenta, .35f);
                    DrawCenterString(batch, "Try breating next level mohaha!", screen.GetCamera().GetPosition().Y + screen.GetCamera().GetHeight() / 2, Color.Maroon, .3f);
                    break;
                default:
                    break;
            }
        }

        public void DrawCenterString(SpriteBatch batch, string text, float y, Color color, float scale = 1)
        {
            batch.DrawString(Assets.font, text, 
                    new Vector2(
                         screen.GetCamera().GetWidth() / 2 - ((Assets.font.MeasureString(text).Length() / 2) * scale), y),
                         color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void DrawCenterString(SpriteBatch batch, string text, float y, float scale = 1)
        {
            DrawCenterString(batch, text, y, Color.White, scale);
        }
    }
}
