using Kriptok.Games.Alien.Scenes;
using Kriptok.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kriptok.Div.Scenes;
using Kriptok.IO;

namespace Kriptok.Games.Alien
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
#if DEBUG || SHOWFPS
            // Cargo la configuración del juego.
            //Config.Load<BaseConfiguration>().Mute();
#endif

            Engine.Start(new DivIntroVideoScene<IntroScene>(true), p =>
            //Engine.Start(new IntroScene(), p =>
            //Engine.Start(new LevelScene(), p =>
            //Engine.Start(new MainMenuScene(), p =>
            //Engine.Start(new CreditsScene(), p =>
            //Engine.Start(new TestScene(), p =>
            {                
                p.Mode = WindowSizeEnum.M640x480;
                p.FullScreen();
                p.OpenMidiNotePlayer();
                p.Title = "Alien Suprimer | Kriptok";
                p.TimerInterval = 30;
            });
        }
    }
}
