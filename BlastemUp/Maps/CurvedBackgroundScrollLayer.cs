using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Core;
using Kriptok.Regions.Scroll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.BlastemUp.Maps
{
    internal class CurvedBackgroundScrollLayer : TransformedScrollLayer
    {
        private int[] array;
        private readonly int[][] table;

        public CurvedBackgroundScrollLayer(Rectangle region)
            : base(region, FastBitmap.Load(typeof(CurvedBackgroundScrollLayer).Assembly,
                "Assets.Images.Backgrounds.Background00.png"), new Vector2<bool>(true, true))
        {
            var hw = region.Size.Width / 2;
            var hh = region.Size.Height / 2;

            var modifier = 1f / (region.Size.Height / 4f + 150f);

            var screen = new int[region.Size.Height, region.Size.Width];

            for (int i = 0; i < screen.GetLength(1); i++)
            {
                var u = i - hw;
                var au = Math.Abs(u);
                int u1 = 0;
                if (u != 0)
                {
                    u1 = -(u / au);
                }

                for (int j = 0; j < screen.GetLength(0); j++)
                {
                    if (u != 0)
                    {
                        screen[j, i] = ((float)(u1 * Math.Pow(au, Math.Abs(j - hh) * modifier)) - u1).Round();
                    }
                }
            }

            table = screen.ToJaggedArray();
        }

        protected override void OnScanLine(int index)
        {
            base.OnScanLine(index);

            array = table[index];
        }

        protected override uint SampleFor(int u, int x, int y)
        {
            return base.SampleFor(u, x + array[u], y);
        }
    }
}
