using MAH_Pacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Scene2D
{
    public class Scene
    {
        private Dictionary<string, Actor> actors;
        private Camera2D camera;

        public Scene(Camera2D camera)
        {
            this.actors = new Dictionary<string, Actor>();
            this.camera = camera;
        }

        public void Update(float delta)
        {
            foreach (var actor in actors)
            {
                actor.Value.Update(delta);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront,
                      BlendState.AlphaBlend,
                      SamplerState.LinearClamp,
                      null,
                      null,
                      null,
                      camera.Update().GetMatrix());

            foreach (var actor in actors)
            {
                actor.Value.Draw(batch);
            }
            batch.End();
        }

        public float GetWidth()
        {
            return camera.GetWidth();
        }

        public float GetHeight()
        {
            return camera.GetHeight();
        }

        public void Add(string name, Actor actor)
        {
            actor.SetScene(this);
            actor.name = name;
            actors.Add(name, actor);
        }

        public Actor Find(string name)
        {
            return actors[name];
        }

        public void Remove(Actor actor)
        {
            actors.Remove(actor.name);
        }

        public Vector2 Unproject(int x, int y)
        {
            return camera.Unproject(x, y);
        }
    }
}
