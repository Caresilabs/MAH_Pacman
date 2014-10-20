using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Scene2D
{
    public interface TouchListener
    {
        void TouchDown(Vector2 mouse);

        void TouchUp(Vector2 mouse);

        void TouchLeave(Vector2 mouse);

        void KeyDown(Keys key);

        void KeyReleased(Keys key);
    }

    public class Actor : TouchListener
    {
        public TouchListener listener;
        public bool isTouched;
        public string name;

        private List<Actor> actors;
        private Scene scene;
        private Rectangle bounds;
        
        public Actor(float x, float y, float width, float height)
        {
            this.bounds = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.actors = new List<Actor>();
            this.isTouched = false;
        }

        public void Update(float delta)
        {
            UpdateInput();

            foreach (var actor in actors)
            {
                actor.Update(delta);
            }
        }

        private void UpdateInput()
        {
            Vector2 mouse = scene.Unproject(Mouse.GetState().X, Mouse.GetState().Y);
            if (InputHandler.Clicked())
            {
                if (bounds.Contains((int)mouse.X, (int)mouse.Y))
                    TouchUp(mouse);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (bounds.Contains((int)mouse.X, (int)mouse.Y))
                    TouchDown(mouse);
            }

            if (isTouched)
            {
                if (!bounds.Contains((int)mouse.X, (int)mouse.Y))
                    TouchLeave(mouse);
            }

            foreach (var key in Keyboard.GetState().GetPressedKeys())
            {
                if (InputHandler.KeyDown(key))
                    KeyDown(key);
            }
            
        }

        public virtual void Draw(SpriteBatch batch)
        {
            foreach (var actor in actors)
            {
                actor.Draw(batch);
            }
        }

        public void SetSize(float width, float height)
        {
            this.bounds.Width = (int)width;
            this.bounds.Height = (int)height;
        }

        public void SetPosition(float x, float y)
        {
            this.bounds.X = (int)x;
            this.bounds.Y = (int)y;
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
        }

        // INPUT
        public virtual void TouchDown(Vector2 mouse)
        {
            isTouched = true;
            if (listener != null) listener.TouchDown(mouse);
        }

        public virtual void TouchUp(Vector2 mouse)
        {
            isTouched = false;
            if (listener != null) listener.TouchUp(mouse);
        }

        public virtual void TouchLeave(Vector2 mouse)
        {
            isTouched = false;
            if (listener != null) listener.TouchLeave(mouse);
        }

        public virtual void KeyDown(Keys key)
        {
            if (listener != null) listener.KeyDown(key);
        }

        public virtual void KeyReleased(Keys key)
        {
            if (listener != null) listener.KeyReleased(key);
        }
    }
}
