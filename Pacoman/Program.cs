using Kriptok.Core;
using Kriptok.Div.Scenes;
using Kriptok.IO;
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
            Config.Load<BaseConfiguration>().Mute();
#endif                  
            Engine.Start(new DivIntroVideoScene<IntroScene>(false), p =>            
            {
                p.Title = "Pacoman | Kriptok";          
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 24;
            });
        }
    }
}
