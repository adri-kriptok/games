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
        public OpenSpaceScrollRegion(Rectangle region)
            : base(region, new StarsScrollLayer())
        {
            AddLayer(new GdipBrushScrollLayer(GetBmp(Color.DarkCyan, 100), true, true)
            {
                ScaleX = 2f,
                ScaleY = 2f,

                ReScaleX = 0.5f,
                ReScaleY = 0.5f,
                Priority = -20000
            });

            AddLayer(new GdipBrushScrollLayer(GetBmp(Color.DarkBlue, 200), true, true)
            {
                ScaleX = 4f,
                ScaleY = 4f,

                ReScaleX = 0.25f,
                ReScaleY = 0.25f,
                Priority = - 10000
            });
        }

        public override int GetLevelHeight() => GlobalConsts.MaxLevelSize;

        private class StarsScrollLayer : GdipBrushScrollLayer
        {
            public StarsScrollLayer() : base(GetBmp(Color.Gray, 50), true, true)
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
