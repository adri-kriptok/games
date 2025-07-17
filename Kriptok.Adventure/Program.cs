using Kriptok.Adventure.Scenes.Maps.Map00;
using Kriptok.Adventure.Scenes.Maps.Map01;
using Kriptok.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Adventure
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new Map00(), s =>
            {
                s.Mode = WindowSizeEnum.W226x128;
                //s.FullScreen();
                s.Title = "Kriptok | Adventure";
                //s.TimerInterval = 16;
            });
        }
    }

    public class Consts
    {
        public const float SpeedMultiplier = 1f / 15f;
    }
}
