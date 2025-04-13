using Billiard.Scenes;
using Kriptok.Core;
using System;
using Kriptok.IO;
using Kriptok.Div.Scenes;

namespace Kriptok.Games.Billar
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
            Engine.Start(new TitleScene(), p =>
#else
            Engine.Start(new DivIntroVideoScene<TitleScene>(false)
            {
                FitToScreen = true
            }, p =>
#endif
            {
                p.Title = "Billar | Kriptok";
                p.CaptureMouse();
                p.Mode = WindowSizeEnum.M640x480;
                p.ExtractMidiPlayer();
                p.TimerInterval = 15;
            });
        }
    }
}
