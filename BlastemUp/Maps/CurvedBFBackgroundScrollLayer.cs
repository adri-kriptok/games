using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kriptok.Games.BlastemUp.Maps
{
    internal class CurvedBFBackgroundScrollLayer : GdipBrushScanlineScrollLayer
    {
        private readonly float[] array;

        public CurvedBFBackgroundScrollLayer(Rectangle region)
            : base(region, Resource.Get(typeof(CurvedBFBackgroundScrollLayer).Assembly,
                "Assets.Images.Backgrounds.Background00.png"), true, true)
        {
            Antialias = true;
            
            array = new float[region.Size.Height];

            var j = -region.Size.Height / 2;

            var tot = region.Size.Height * 0.5f;
            for (int i = 0; i < array.Length; i++, j++)
            {
                array[i] = 2f - (float)Math.Sqrt(Math.Abs(Math.Cos(j / (float)tot)));
            }
        }

        protected override void OnScanline(IRenderContext context, Matrix transform, int y)
        {            
            transform.Scale(array[y], 1f);            
        }
    }
}
