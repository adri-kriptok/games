﻿using Kriptok;
using Kriptok.Core;
using Kriptok.IO;
using PerdidoEnElTiempo.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerdidoEnElTiempo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config.Load<BaseConfiguration>().Mute();
#if DEBUG
#endif
            Engine.Start(new InitScene(), s =>
            {
                s.FullScreen = false;
                s.Mode = WindowSizeEnum.M320x200To240;
                s.Title = "Perdido en el Tiempo | Kriptok";
            });
        }
    }
}
