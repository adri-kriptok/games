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
#if DEBUG || SHOWFPS
            Config.Load<BaseConfiguration>().Mute();
#endif
            Engine.Start(new Map00Scene(), p =>
            //Engine.Start(new Map01Scene(), p =>
            //Engine.Start(new Map02Scene(), p =>
            {
                p.FullScreen();
                p.Mode = WindowSizeEnum.W340x192;
                //p.Mode = WindowSizeEnum.W384x216;
                p.Title = "Tehuelche | Kriptok";
                p.OpenMidiNotePlayer();
                //p.TimerInterval = 30;
                p.CaptureMouse();
            });
        }        
    }
}
