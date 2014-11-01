using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MAH_Pacman.Model;
using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Systems;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Scene2D;
using MAH_Pacman.Tools;

namespace MAH_Pacman.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class HighscoreScreen : Screen
    {
        private int[] highscores;

        public override void Init()
        {
            this.highscores = HighscoreManager.GetHighscores();
        }


        public override void Update(float delta)
        {
            if (InputHandler.KeyReleased(Keys.M))
                SetScreen(new MainMenuScreen());
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);

            batch.Begin();

            batch.DrawString(Assets.font, "HIGHSCORES", new Vector2(GetGraphics().Viewport.Width / 2 - 120, 40), Color.White);

            for (int i = 0; i < highscores.Length; i++)
            {
                batch.DrawString(Assets.font, highscores[i].ToString(), new Vector2(GetGraphics().Viewport.Width / 2 - 40, 150 + i * 60), Color.White);
            }
            batch.End();
        }

        public override void Dispose()
        {

        }
    }
}
