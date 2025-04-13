using Fostiator.Scenes;
using Kriptok.IO;
using Kriptok.Core;
using System;
using Kriptok;

namespace Fostiator
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
            
            Engine.Start(new IntroScene(), p =>
            {
                p.Title = "DIV - Fostiator | Kriptok";
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 16;
            });
        }
    }
}

