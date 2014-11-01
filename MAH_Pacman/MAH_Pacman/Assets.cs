using MAH_Pacman.Scene2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman
{
    /**
* Assets loads all the needed assets and a non cumbersome way to retrieve them
*/
    public class Assets
    {
        public const bool SOUND = false;

        private static Dictionary<String, TextureRegion> regions;
        private static ContentManager manager;

        public static Texture2D items;
        public static Texture2D ui;

        public static SpriteFont font;

        public static SoundEffect bombSound;
        public static SoundEffect missSound;
        public static Song music;

        public static void Load(ContentManager manager)
        {
            Assets.manager = manager;
            regions = new Dictionary<string, TextureRegion>();

            // load our sprite sheet
            items = manager.Load<Texture2D>("Graphics/items");
            ui = manager.Load<Texture2D>("Graphics/pacman");

            // Load our assets regions
            LoadRegion("pacman", items, 16, 0, 16, 16);
            LoadRegion("energizer", items, 64, 0, 16, 16);

            LoadRegion("fruit1", items, 0, 96, 16, 16);
            LoadRegion("fruit2", items, 0, 96, 16, 16);
            LoadRegion("fruit3", items, 0, 96, 16, 16);
            LoadRegion("fruit4", items, 0, 96, 16, 16);

            // Load tiles
            LoadRegion("tileOut0", ui, 0, 192, 24, 24);
            LoadRegion("tileOut1", ui, 0, 192, 24, 24);
            LoadRegion("tileOut2", ui, 0, 192, 24, 24);
            LoadRegion("tileOut3", ui, 0, 192, 24, 24);

            LoadRegion("tileIn0", ui, 24, 192, 24, 24);
            LoadRegion("tileIn1", ui, 24, 192, 24, 24);
            LoadRegion("tileIn2", ui, 24, 192, 24, 24);
            LoadRegion("tileIn3", ui, 24, 192, 24, 24);

            LoadRegion("tileWall0", ui, 48, 192, 24, 24);
            LoadRegion("tileWall1", ui, 48, 192, 24, 24);
            LoadRegion("tileWall2", ui, 48, 192, 24, 24);
            LoadRegion("tileWall3", ui, 48, 192, 24, 24);

            // others
            LoadRegion("pixel", items, 133, 0, 2, 2);

            // Load UI
            LoadRegion("title", ui, 0, 0, 512, 64);
            LoadRegion("uiContainer1", ui, 0, 64, 320, 128);
            LoadRegion("button", ui, 320, 64, 192, 64);

            // Load font 
            font = manager.Load<SpriteFont>("Font/font");

            // load sound
            if (SOUND)
            {
                //missSound = manager.Load<SoundEffect>("Audio/miss");
                music = manager.Load<Song>("Audio/music");
                MediaPlayer.Volume = .8f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(music);
            }
        }

        private static void LoadRegion(string name, Texture2D tex, int x, int y, int width, int height)
        {
            regions.Add(name, new TextureRegion(tex, x, y, width, height));
        }

        public static TextureRegion GetRegion(string name)
        {
            return regions[name];
        }

        public static void Unload()
        {
            manager.Dispose();
        }
    }
}
