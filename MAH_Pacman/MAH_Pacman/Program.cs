using System;

namespace MAH_Pacman
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Start game = new Start())
            {
                game.Run();
            }
        }
    }
#endif
}

