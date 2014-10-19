using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MAH_Pacman.Model;
using MAH_Pacman.Controller;

namespace MAH_Pacman
{
    public class Start : Microsoft.Xna.Framework.Game
    {
        public const string GAME_NAME = "Pacmoma";

        private static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Screen currentScreen;

        private float aspectRatio;

        public Start()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            int scale = 2;

            graphics.PreferredBackBufferWidth = 224 * scale;
            graphics.PreferredBackBufferHeight = 288 * scale;

            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.Title = GAME_NAME + " by [Simon Bothen]"; //  set title to our game name
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.load(Content);

            // init startup screen
            setScreen(getStartScreen());
        }

        protected override void UnloadContent()
        {
            Assets.unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // get second between last frame and current frame, used for fair physics manipulation and not based on frames
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // then update the screen
            currentScreen.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw screen
            currentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void setScreen(Screen newScreen)
        {
            if (newScreen == null) return;

            // Dispose old screen
            if (currentScreen != null)
                currentScreen.Dispose();

            // init new screen
            currentScreen = newScreen;
            newScreen.SetGame(this);
            newScreen.SetGraphics(GraphicsDevice);
            currentScreen.Init();
        }

        public static void changeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        private Screen getStartScreen()
        {
            return new GameScreen();
        }
    }
}
