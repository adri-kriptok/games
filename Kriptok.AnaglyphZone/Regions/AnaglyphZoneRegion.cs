using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Regions.Pseudo3D;
using System.Drawing;

namespace Kriptok.AZ.Regions
{
    internal class AnaglyphZoneRegion : Pseudo3DRegion
    {
        private readonly AnaglyphPseudo3DHandler<AnaglyphZoneRegion, Pseudo3DRenderContext> anaglyph3D;

        public AnaglyphZoneRegion(Rectangle rectangle) : base(rectangle)
        {
            anaglyph3D = new AnaglyphPseudo3DHandler<AnaglyphZoneRegion, Pseudo3DRenderContext>(this);           
        }

        public float DistanceModifier 
        {
            get => anaglyph3D.DistanceModifier;
            set => anaglyph3D.DistanceModifier = value;
        }

        protected override void Render(Graphics g, EntityBase[] entities)
        {            
            anaglyph3D.Render(g, g2 => base.Render(g2, entities));            
        }

        protected override Pseudo3DRenderContext NewRenderContext(Graphics g, Camera camera)
        {
            return anaglyph3D.NewRenderContext(camera, newCam => base.NewRenderContext(g, newCam));
        }
    }
}
