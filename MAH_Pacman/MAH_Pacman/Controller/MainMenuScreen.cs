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
    public class MainMenuScreen : Screen, EventListener
    {
        private Scene scene;

        public override void Init()
        {
            this.scene = new Scene(new Camera2D(GetGraphics(),224, 288), this);

            UIButton button = new UIButton("Play!", scene.GetWidth()/2, 100, 1);
            scene.Add("start", button);

            UIButton highscores = new UIButton("Highscores", scene.GetWidth() / 2, 150, 1);
            scene.Add("highscores", highscores);

            UIButton exit = new UIButton("Level Editor", scene.GetWidth() / 2, 200, 1);
            scene.Add("editor", exit);

            UIImage title = new UIImage(Assets.GetRegion("title"), scene.GetWidth() / 2, 60, .5f);
            scene.Add("title", title);
        }


        public override void Update(float delta)
        {
            scene.Update(delta);
        }

        public override void Draw(SpriteBatch batch)
        {
            GetGraphics().Clear(Color.Black);
            scene.Draw(batch);
        }

        public override void Dispose()
        {

        }


        public void EventCalled(Events e, Actor actor)
        {
            if (e == Events.TouchUp)
            {
                if (actor.name == "editor")
                {
                    SetScreen(new LevelEditorScreen());
                }
                if (actor.name == "highscores")
                {
                    SetScreen(new HighscoreScreen());
                }
                if (actor.name == "start")
                {
                    SetScreen(new GameScreen());
                }
            }
        }
    }
}
