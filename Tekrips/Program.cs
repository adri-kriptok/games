using Kriptok;
using Kriptok.Core;
using System;
using Tekrips.Scenes;

namespace Tekrips
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Engine.Start(new BoardScene(), p => 
            {
                p.FullScreen = false;
                p.Mode = WindowSizeEnum.M320x200;
                p.Title = "Tekrips | Kriptok";
				p.ExtractMidiPlayer();
            });
        }
    }
}
