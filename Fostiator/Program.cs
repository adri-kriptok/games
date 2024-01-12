using Kriptok.Games.Dgs.Fostiator.Scenes;
using Kriptok.IO;
using Kriptok.Core;
using System;

namespace Kriptok.Games.Div.Fostiator
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

            //Engine.Start(new OptionsActivity(), p =>
            Engine.Start(new IntroScene(), p =>
            {
                p.Title = "DIV - Fostiator | Kriptok";
                p.FullScreen = false;
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 16;
            });
        }
    }
}

