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
#if DEBUG
            Config.Get<BaseConfiguration>().Mute();
#endif
            //Engine.Start(new TheBeachMapScene(), p =>
            Engine.Start(new IntroScene(), p =>
            {
                p.FullScreen = true;                
                p.Mode = Core.WindowSizeEnum.W340x192;                
                p.Title = "Intruder | Kriptok";
                p.OpenMidiNotePlayer();
                p.ExtractMidiPlayer();
            });
        }
    }
}
