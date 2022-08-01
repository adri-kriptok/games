using Kriptok.Core;
using System;

namespace Kriptok.Asteroids
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new TitleScene(), p =>
            {
                p.Title = "Asteroids | Kriptok";
                p.FullScreen = false;
                p.Mode = WindowSizeEnum.M960x600;
                p.TimerInterval = 25;
            });
        }
    }
}
