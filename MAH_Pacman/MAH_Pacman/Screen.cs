using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MAH_Pacman
{
    public abstract class Screen
    {
        private GraphicsDevice graphics;

        private Viewport defaultViewPort;

        private Start game;

        public abstract void Init();

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch batch);

        public abstract void Dispose();

        public void SetScreen(Screen newScreen)
        {
            game.setScreen(newScreen);
        }

        public void SetGame(Start game)
        {
            this.game = game;
        }

        public Game GetGame()
        {
            return game;
        }

        public GraphicsDevice GetGraphics()
        {
            return graphics;
        }

        public Viewport GetDefaultViewPort()
        {
            return defaultViewPort;
        }

        public void SetGraphics(GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        public void SetDefaultViewPort(Viewport viewport)
        {
            this.defaultViewPort = viewport;
        }
    }
}
