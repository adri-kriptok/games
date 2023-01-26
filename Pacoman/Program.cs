using Kriptok.Core;
using Pacoman.Scenes;
using System;

namespace Kriptok.Games.Pacoman
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            //Config.Load<BaseConfiguration>().Mute();
#endif                  
            Engine.Start(new IntroScene(), p =>
            // Engine.Start(new GameScene(0), p =>
            {
                p.Title = "Pacoman | Kriptok";
                p.FullScreen = false;                
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 32;
            });
        }
    }
}
