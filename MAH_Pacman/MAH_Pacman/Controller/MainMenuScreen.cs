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
    public class MainMenuScreen : Screen
    {
        private Scene scene;

        public MainMenuScreen()
        {
        }

        public override void Init()
        {
            this.scene = new Scene(new Camera2D(GetGraphics(),224, 288));

            UIButton button = new UIButton("hello", 10, 10, 2);
            
            scene.Add("main", button);
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

    }
}
