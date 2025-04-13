using Kriptok.Extensions;
using Kriptok.Regions.Buffered;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Regions
{
    internal class StarsRegion : AnaglyphZoneRegion
    {
        private readonly Rectangle rectangle;
        private Bitmap buffer1;
        private Bitmap buffer2;
        private Bitmap buffer3;

        private TextureBrush brush1;
        private TextureBrush brush2;
        private TextureBrush brush3;

        public StarsRegion(Rectangle rectangle) : base(rectangle)
        {
            this.rectangle = rectangle;
            buffer1 = new Bitmap(rectangle.Width, rectangle.Height);
            buffer2 = new Bitmap(rectangle.Width, rectangle.Height);
            buffer3 = new Bitmap(rectangle.Width, rectangle.Height);

            var w = rectangle.Width - 2;
            var h = rectangle.Height - 2;
            for (int i = 0; i < 50; i++)
            {
                SetPixel(buffer1, Rand.Next(0, w), Rand.Next(0, h), Color.Fuchsia);
                SetPixel(buffer2, Rand.Next(0, w), Rand.Next(0, h), Color.Fuchsia);
                SetPixel(buffer3, Rand.Next(0, w), Rand.Next(0, h), Color.Fuchsia);
            }

            brush1 = new TextureBrush(buffer1);
            brush2 = new TextureBrush(buffer2);
            brush3 = new TextureBrush(buffer3);
        }

        private void SetPixel(Bitmap buffer, int x, int y, Color color)
        {
            buffer.SetPixel(x, y, color);
            buffer.SetPixel(x+1, y, color);
            buffer.SetPixel(x, y+1, color);
            buffer.SetPixel(x+1, y+1, color);
        }

        protected override void Render(Pseudo3DRenderContext context, IEnumerable<IRenderizable> views)
        {
            var camLocation = context.Camera.Location;

            var g = context.Graphics;

            g.SavingState(() =>
            {
                brush3.ResetTransform();
                brush3.TranslateTransform(-(camLocation.X * 0.1f).Round(), (camLocation.Z * 0.1f).Round());
                g.FillRectangle(brush3, rectangle);

                brush2.ResetTransform();
                brush2.TranslateTransform(-(camLocation.X * 0.2f).Round(), (camLocation.Z * 0.2f).Round());
                g.FillRectangle(brush2, rectangle);

                brush1.ResetTransform();
                brush1.TranslateTransform(-(camLocation.X * 0.3f).Round(), (camLocation.Z * 0.3f).Round());
                g.FillRectangle(brush1, rectangle);
            });

            base.Render(context, views);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (brush1 != null) 
            {
                brush1.Dispose();
                brush1 = null;
            }

            if (brush2 != null)
            {
                brush2.Dispose();
                brush2 = null;
            }

            if (brush3 != null)
            {
                brush3.Dispose();
                brush3 = null;
            }

            if (buffer1 != null)
            {
                buffer1.Dispose();
                buffer1 = null;
            }

            if (buffer2 != null)
            {
                buffer2.Dispose();
                buffer2 = null;
            }

            if (buffer3 != null)
            {
                buffer3.Dispose();
                buffer3 = null;
            }
        }
    }
}
