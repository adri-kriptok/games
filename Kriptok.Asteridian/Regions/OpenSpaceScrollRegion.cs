using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Regions
{
    class OpenSpaceScrollRegion : Asteridian3DScrollBase
    {
        public OpenSpaceScrollRegion(LevelSceneBase level, Rectangle region)
            : base(level, region, new StarsScrollLayer())
        {

            AddLayer(new GdipBrushScrollLayer(GetBmp(Color.LightGray, 10), true, true)
            {
                ScaleX = 0.25f,
                ScaleY = 0.25f,

                ReScaleX = 4f,
                ReScaleY = 4f,
                Priority = -10000
            });

            //AddLayer(new GdipBrushScrollLayer(GetBmp(Color.DarkOrange, 25), true, true)
            //{
            //    ScaleX = 0.25f,
            //    ScaleY = 0.25f,

            //    ReScaleX = 4f,
            //    ReScaleY = 4f,
            //    Priority = -20000
            //});

            AddLayer(new GdipBrushScrollLayer(GetBmp(Color.DarkCyan, 50), true, true)
            {
                ScaleX = 0.5f,
                ScaleY = 0.5f,

                ReScaleX = 2f,
                ReScaleY = 2f,
                Priority = -20000
            });

        }

        public override int GetLevelHeight() => GlobalConsts.MaxLevelSize;

        private class StarsScrollLayer : GdipBrushScrollLayer
        {
            public StarsScrollLayer() : base(GetBmp(Color.DarkBlue, 100), true, true)
            {
            }

            // protected override void Render(ScrollRenderContextBase context)
            // {
            //     context.CalculateScreenCoords(Vector2F.Empty);
            //     //throw new NotImplementedException();
            // }
        }

        private static FastBitmap GetBmp(Color color, int count)
        {
            var fb = FastBitmap.CreateBySize(256, 256, 0x0u.ToColor());

            for (int i = 0; i < count; i++)
            {
                fb.SetPixel(Rand.Next(0, 255), Rand.Next(0, 255), color);
            }
#if DEBUG
                
#endif
            return fb;
        }
    }
}
