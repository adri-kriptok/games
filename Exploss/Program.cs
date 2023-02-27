using Exploss.Scenes;
using Kriptok;
using Kriptok.Core;
using Kriptok.IO;
using System;

namespace Exploss
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Global.ScoreRecord = MaxScore.Load();
#if DEBUG
            Config.Load<ExplossConfiguration>().Mute();
#endif
            Engine.Start(new MenuScene(), p =>
            {
                p.Title = "Exploss | Kriptok";
                p.FullScreen = false;
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 50;
            });
        }
    }
}