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
            Config.Load<BaseConfiguration>().Mute();
#if DEBUG
#endif
            Engine.Start(new GameScene(1), p =>
            {
                p.Title = "Exploss | Kriptok";
                p.FullScreen = true;
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 50;
            });
        }
    }
}