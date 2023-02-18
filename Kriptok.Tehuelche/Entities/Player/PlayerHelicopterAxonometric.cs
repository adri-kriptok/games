using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Regions;

namespace Kriptok.Tehuelche.Entities
{
    internal class PlayerHelicopterAxonometric : PlayerHelicopterBase
    {
        private readonly TehuelcheMapRegionAxonometric region;
        private const float modifier = 0.25f;

        public PlayerHelicopterAxonometric(TehuelcheMapRegionAxonometric region, float x, float y) 
            : base(region, x, y, 0.35f)
        {            
            this.region = region;
        }

        protected override void OnFrame()
        {     
            var ang = region.RotationWithMouseHorizontally(1) * 0.05f;
            
            Angle.X += ang * modifier;
            Angle.Z += ang * modifier;

            base.OnFrame();

            region.CameraHeight = (region.CameraHeight + Location.Z) * 0.5f/*- 16f*/;
            region.Rotation = -CameraAngle - MathHelper.HalfPIF;
        }

        internal override Vector2F GetAngles()
        {
            return new Vector2F(Angle.Y - ThirdPersonAngleModifier, Angle.Z);
        }

        protected override bool IsVisible() => true;
    }
}
