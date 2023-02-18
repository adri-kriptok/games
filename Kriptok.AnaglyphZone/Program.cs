using Kriptok.Core;
using Kriptok.IO;
using Kriptok.AZ.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kriptok.Vector3D.Scenes;

namespace Kriptok.AZ
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            // Cargo la configuración del juego.
            Config.Load<BaseConfiguration>().Mute();
#endif

            // Engine.Start(new StarsScene(), s =>
            Engine.Start(new TitleScene(), s =>
            {
                s.FullScreen = true;
                s.Mode = WindowSizeEnum.W340x192;
                s.Title = "Anaglyph Zone | Kriptok";
            });
        }
    }
}
