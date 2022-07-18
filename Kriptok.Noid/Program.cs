using Kriptok.Core;
using Kriptok.Noid.Scenes;
using System;

namespace Kriptok.Noid
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Engine.Start(new LevelScene(Consts.FirstLevel, true), s =>
            {
                s.FullScreen = false;
                s.Mode = WindowSizeEnum.M320x200;
                s.Title = "Kriptok - Noid";
                s.TimerInterval = 16;
            });
        }
    }
}
