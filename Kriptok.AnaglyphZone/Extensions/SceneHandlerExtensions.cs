using Kriptok.AZ;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Vector3D.Extensions
{
    internal static class SceneHandlerExtensions
    {
        public static void WriteScoreboard(this SceneHandler h)
        {
            h.Write(AZConsts.TextFont, h.ScreenRegion.Size.Width, 0, () => 
                $"Record: {Global.Record}").RightTop();
            h.Write(AZConsts.TextFont, 0, 0, () => 
                $"Score: {Global.Score}").LeftTop();
        }
    }
}
