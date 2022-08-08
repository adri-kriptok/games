using Kriptok;
using Kriptok.Audio;
using Kriptok.Core;
using Kriptok.IO;
using Noid.Scenes;
using System;

namespace Noid
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#endif
            Global.ResetValues();            
            Engine.Start(new IntroScene(), s =>
            {
                s.FullScreen = false;
                s.Mode = WindowSizeEnum.M320x200;
                s.Title = "Noid | Kriptok";
                s.TimerInterval = 16;
                s.OpenMidiNotePlayer();
            });            
        }
    }
}
