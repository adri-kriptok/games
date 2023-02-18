using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Regions.VoxelSpace;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Regions
{
    internal class TehuelcheYBuffer : VoxelYBuffer1
    {
        private static ushort[] resetValues = Array.Empty<ushort>();

        public TehuelcheYBuffer(Size size, int screenHeight) : base(size)
        {
            resetValues = InitResetValues(screenHeight);
        }

        public override void Reset()
        {
#if DEBUG || SHOWFPS
            base.Reset();
#endif
            base.SetBufferState(0, resetValues);
        }

        private ushort[] InitResetValues(int screenHeight)
        {
            using (var bmp = new FastBitmap(GetType().Assembly, "Assets.Images.Hud.Hud.png"))
            {
                var tc = bmp.GetTransparentColor().Value.ToUInt32();
                var diff = screenHeight - bmp.Size.Height;

                var arr = new ushort[bmp.Size.Width];

                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = 0; j < bmp.Size.Height; j++)
                    {
                        if (bmp.Sample(i, j) != tc)
                        {
                            arr[i] = (ushort)(diff + j);
                            break;
                        }
                    }
                }

                return arr;
            }
        }
    }
}
