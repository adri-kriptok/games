using Kriptok.Core;
using Kriptok.Helpers;
using Kriptok.Maps.Editor;
using Kriptok.Maps.Tiles.Editor;
using Kriptok.Noid.Scenes;
using System;
using System.Collections.Generic;

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

            // var lev = TiledMapConfig.Load(typeof(Program).Assembly, "Level00.noidx");
            // 
            // for (int j = 1; j <= 12; j++)
            // {
            //     var list = new List<uint>();
            //     var levelData = ResourcesHelper.GetBytes(typeof(Program).Assembly, $"Assets.Levels.SCREEN.{j}");
            //     var levelData2 = new int[levelData.Length / 4];
            //     for (int i = 0; i < levelData2.Length; i++)
            //     {
            //         var val = (uint)levelData[i * 4];
            //         if (val == 30)
            //         {
            //             val = 8;
            //         }
            //         else if (val == 20)
            //         {
            //             val = 7;
            //         }
            //         else if(val == 0)
            //         {
            //             
            //         }
            //         else
            //         {
            //             val = val - 9;
            //         }
            //         list.Add(val);
            //     }
            // 
            //     lev.Data = new TiledMapConfigData(list.ToArray());
            //     lev.Save($"Level{j:00}.noidx");
            // }
        }
    }
}
