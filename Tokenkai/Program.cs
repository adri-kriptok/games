using Kriptok;
using Kriptok.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tokenkai.Scenes;

namespace Tokenkai
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new Level1Scene(), p =>
            {
                p.Title = "Tokenkai";
                p.Mode = WindowSizeEnum.M640x480;
                p.FullScreen();
                p.CaptureMouse();
            });
        }
    }
}
