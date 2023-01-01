using Kriptok.Intruder.Scenes;
using Kriptok.Intruder.Scenes.Maps.Map00_Test;
using Kriptok.Intruder.Scenes.Maps.Map01_TheBeach;
using Kriptok.Intruder.Scenes.Missions;
using Kriptok.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Intruder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Config.Get<BaseConfiguration>().Mute();

            //Engine.Start(new Mission01_TheBeach(), p =>
            //Engine.Start(new TheBeachMapScene(), p =>
            //Engine.Start(new TestMapScene(), p =>
            Engine.Start(new IntroScene(), p =>
            {
#if DEBUG
                p.FullScreen = false;
#else
                p.FullScreen = true;
#endif
                //p.Mode = Core.WindowSizeEnum.W226x128;
                p.Mode = Core.WindowSizeEnum.W340x192;
                //p.Mode = Core.WindowSizeEnum.W680x384;
                p.Title = "Intruder | Kriptok";
                p.OpenMidiNotePlayer();
                p.ExtractMidiPlayer();
            });
        }
    }
}
