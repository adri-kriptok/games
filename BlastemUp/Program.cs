using Kriptok.Games.BlastemUp.Scenes;
using Kriptok.IO;
using Kriptok.Core;
using System;
using System.Windows.Forms;
using Kriptok.Drawing;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kriptok.Games.BlastemUp
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
#else
            Config.Load<BaseConfiguration>();
#endif            

            Engine.Start(new TitleScreenScene(), p =>
            {
                p.Title = "DIV - Blast'em Up | Kriptok";
                p.Mode = WindowSizeEnum.M640x480;
                p.FullScreen();
                p.TimerInterval = 45;
            }); return;

            //Merge(
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710000.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710001.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710002.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710003.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710004.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710005.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710006.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710007.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710008.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710009.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710010.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710011.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710012.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710013.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710014.bmp",
            //   "E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\TRAFFICT_SHIP_710015.bmp");

        }

        //private static void Merge(params string[] args)
        //{
        //    int w, h;
        //    using (var first = new Bitmap(args[0]))
        //    {
        //        w = first.Width;
        //        h = first.Height;
        //    }

        //    using (var fb = FastBitmap.CreateBySize(w*4, h*4, null))
        //    {
        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            var x = (i % 4) * w;
        //            var y = (i / 4) * h;

        //            using (var bmp = new Bitmap(args[i]))
        //            {
        //                using (var a = FastBitmap.CreateFrom(bmp))
        //                {
        //                    fb.BlitImage(a, x, y);
        //                }
        //            }                    
        //            System.IO.File.Delete(args[i]);
        //        }
        //        fb.ToBitmap().Save($"E:\\_kriptok.games3\\DivGamesStudio\\Kriptok.Sdk.DivGamesStudio.BlastemUp\\Assets\\Bryce\\traffic_ship\\a{DateTime.Now.Ticks}.bmp", ImageFormat.Bmp);
        //    }
        //}
    }    
}
