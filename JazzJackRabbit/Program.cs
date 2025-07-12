using Kriptok.Core;
using Kriptok.JazzJackRabbit.Scenes;
using Kriptok.JazzJackRabbit.Scenes.Level00;
using Kriptok.JazzJackRabbit.Scenes.TestLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.JazzJackRabbit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new Level00(), p =>
            {
                p.Mode = WindowSizeEnum.W454x256;
                //p.FullScreen();
                p.Title = "Jazz JackRabbit | Kriptok";
                p.TimerInterval = 14;
            });
        }
    }
}
