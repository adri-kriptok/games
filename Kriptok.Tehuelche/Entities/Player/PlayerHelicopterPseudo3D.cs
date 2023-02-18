using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Regions;
using System.Windows.Forms;

namespace Kriptok.Tehuelche.Entities
{
    internal class PlayerHelicopterPseudo3D : PlayerHelicopterBase
    {
        private const float modifier = 0.25f;

        private readonly TehuelcheMapRegionPseudo3D terrain;

        public PlayerHelicopterPseudo3D(TehuelcheMapRegionPseudo3D region, float x, float y) 
            : base(region, x, y, 0.05f)
        {            
            this.terrain = region;
        }

        /// <summary>
        /// Obtiene o establece si debe utilizar la vista en primera persona.
        /// </summary>
        internal bool UserFirstPersonCamera { get; private set; } = true;

        protected override void OnFrame()
        {
            if (Input.KeyPressed(Keys.Tab))
            {
                UserFirstPersonCamera = !UserFirstPersonCamera;
            }

            var ang = terrain.RotationWithMouseHorizontally(1) * 0.05f;
            terrain.TiltAngle = -ang;
            Angle.X += ang * modifier;
            Angle.Z += ang * modifier;

            base.OnFrame();
        }

        /// <summary>
        /// Obtiene el ángulo de la cámara, para cuando se juega en primera persona.
        /// </summary>        
        internal float GetCameraAngle() => terrain.GetCameraVerticalAngle();

        internal override Vector3F GetShootingDirection()
        {
            if (UserFirstPersonCamera)
            {
                return new Vector3F()
                {
                    X = Angle.X,
                    Y = GetCameraAngle() - 0.1f,
                    Z = CameraAngle
                };
            }
            else
            {
                return base.GetShootingDirection();
            }
        }

        internal override Vector3F GetShootingLocation()
        {
            if (UserFirstPersonCamera)
            {
                var loc = Location;
                loc.Z -= 1f;
                return loc;
            }
            else
            {
                return base.GetShootingLocation();
            }
        }

        internal override Vector2F GetAngles()
        {
            Vector2F angle;
            if (UserFirstPersonCamera)
            {
                angle.X = GetCameraAngle() * 0.5f - MathHelper.QuarterPIF;
                angle.Y = CameraAngle;
            }
            else
            {
                angle.X = Angle.Y - ThirdPersonAngleModifier - GetCameraAngle() * 0.125f;
                angle.Y = Angle.Z;
            }

            return angle;
        }

        protected override bool IsVisible() => !UserFirstPersonCamera;
    }
}
