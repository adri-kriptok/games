using Kriptok.Core;
using Kriptok.IO;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Tehuelche.Scenes;
using Kriptok.Tehuelche.Scenes.Map00;
using Kriptok.Tehuelche.Scenes.Map01;
using Kriptok.Tehuelche.Scenes.Map02;
using System;

namespace Kriptok.Tehuelche
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#endif

            Engine.Start(new Map01Scene(), p =>
            {
                p.FullScreen = true;
                p.Mode = WindowSizeEnum.W340x192;
                p.Title = "Kriptok - Tehuelche";
                p.OpenMidiNotePlayer();
            });
        }        
    }
}
