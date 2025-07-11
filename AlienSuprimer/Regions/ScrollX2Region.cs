using Kriptok.Entities.Base;
using Kriptok.Regions.Buffered;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Axonometric;
using System.Drawing;

namespace Kriptok.Games.Alien.Regions
{
    internal class ScrollX2Region : FixedScrollRegion
    {
        private readonly static Rectangle rect = new Rectangle(0, 0, 320, 240);
        private readonly GdipBackBuffer backBuffer = new GdipBackBuffer(rect);

        public ScrollX2Region()
            : base(rect, new FixedGdipImageScrollLayer(typeof(ScrollX2Region).Assembly, "Assets.Images.MapTexture.png", false, false))
        {
            SetTarget(Cam = new AlienScrollTarget(20, Global.Consts.InitialCameraLocationY));
        }

        public readonly AlienScrollTarget Cam;

        protected override void Render(Graphics g, EntityBase[] entities)
        {
            // Reseteo el clip para que renderice sobre toda la pantalla.             
            g.ResetClip();
            g.ScaleTransform(2f, 2f);
            backBuffer.Render(g, backBufferGraphics => base.Render(backBufferGraphics, entities));
        }
    }
}