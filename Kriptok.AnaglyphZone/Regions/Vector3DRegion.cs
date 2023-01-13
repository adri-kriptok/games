using Kriptok.Core;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Forms;
using Kriptok.Helpers;
using Kriptok.Regions.Buffered;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.AZ.Regions
{
    internal class Vector3DRegion : Pseudo3DRegionBase<Vector3DContext>
    {
        private readonly BackBufferGraphics channelBuffer;
        private readonly ImageAttributes redChannel;
        private readonly ImageAttributes blueChannel;
        private bool renderChannelRed = false;
        
        internal float DistanceModifier = 9.5f;

        public Vector3DRegion(Rectangle rectangle) : base(rectangle)
        {
            this.channelBuffer = new BackBufferGraphics(rectangle);

            var redTransform = new ColorTransform()
            {
                A = 0.5f,
                R = new ColorVector(1f, 0f, 0f),
                G = new ColorVector(0f, 0f, 0f),
                B = new ColorVector(0f, 0f, 0f)
            };
            redChannel = new ImageAttributes();
            redChannel.SetColorMatrix(redTransform.ToColorMatrix());

            var blueTransform = new ColorTransform()
            {
                A = 1f,
                R = new ColorVector(0f, 0f, 0f),
                G = new ColorVector(0f, 0f, 0f),
                B = new ColorVector(0f, 0f, 1f)
            };
            blueChannel = new ImageAttributes();
            blueChannel.SetColorMatrix(blueTransform.ToColorMatrix());
        }

        protected override void Render(Graphics g, EntityBase[] entities)
        {
            channelBuffer.Clear();
            renderChannelRed = false;
            base.Render(channelBuffer.Graphics, entities);

            // ------------------------------------------------------------------
            // Dibujo el canal azul en el objeto general.
            g.CompositingMode = CompositingMode.SourceCopy;
            channelBuffer.RenderOn(g, blueChannel);
            // ------------------------------------------------------------------

            channelBuffer.Clear();
            renderChannelRed = true;
            base.Render(channelBuffer.Graphics, entities);

            // ------------------------------------------------------------------
            // Dibujo el canal rojo en el objeto general.
            g.CompositingMode = CompositingMode.SourceOver;
            channelBuffer.RenderOn(g, redChannel);
            // ------------------------------------------------------------------
        }

        protected override Vector3DContext NewRenderContext(Graphics g, Camera camera)
        {
            var dir = camera.DirectionAngle + MathHelper.HalfPIF;
            var sin = MathHelper.SinF(dir);
            var cos = MathHelper.CosF(dir);

#if DEBUG || SHOWFPS
            if (KeyboardHelper.Key(Keys.D1))
            {
                DistanceModifier -= 0.1f;
                PrintData();
            }
            else if (KeyboardHelper.Key(Keys.D2))
            {
                DistanceModifier += 0.1f;
                PrintData();
            }
            // if (KeyboardHelper.Key(Keys.Q))
            // {
            //     angleModifier -= 0.001f;
            //     PrintData();
            // }
            // else if (KeyboardHelper.Key(Keys.E))
            // {
            //     angleModifier += 0.001f;
            //     PrintData();
            // }
#endif

            if (renderChannelRed)
            {
                // camera.DirectionAngle += angleModifier;
                camera.Location.X += cos * DistanceModifier;
                camera.Location.Y += sin * DistanceModifier;
            }
            else
            {
                // camera.DirectionAngle -= angleModifier;
                camera.Location.X -= cos * DistanceModifier;
                camera.Location.Y -= sin * DistanceModifier;
            }

            return new Vector3DContext(g, camera, this);
        }

#if DEBUG || SHOWFPS
        private void PrintData()
        {
            // Trace.WriteLine($"Distance {distanceModifier}; Angle {angleModifier}");
            Trace.WriteLine($"Distance {DistanceModifier};");
        }
#endif
    }
}
