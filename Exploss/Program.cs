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
#else
            Config.Load<ExplossConfiguration>();
#endif
            Engine.Start(new MenuScene(), p =>
            {
                p.Title = "Exploss | Kriptok";                
                p.Mode = WindowSizeEnum.M640x480;
                p.TimerInterval = 40;
            });
        }
    }
}