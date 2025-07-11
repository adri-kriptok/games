using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Regions;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kriptok.Tehuelche.Entities
{
    internal class PlayerHelicopterPseudo3D : PlayerHelicopterBase
    {
        private const float modifier = 0.25f;

        private readonly TehuelcheMapRegionPseudo3DBase terrain;

        public PlayerHelicopterPseudo3D(TehuelcheMapRegionPseudo3DBase region, Vector2F location) 
            : base(region, location.X, location.Y, 0.05f)
        {            
            this.terrain = region;
        }

        /// <summary>
        /// Obtiene o establece si debe utilizar la vista en primera persona.
        /// </summary>
        internal bool UserFirstPersonCamera { get; private set; } = true;

        //int counter = 0;
        //float acum = 0f;

        protected override void OnFrame(float multiplier)
        {         
            if (Input.KeyPressed(Keys.Tab))
            {
                UserFirstPersonCamera = !UserFirstPersonCamera;
            }

            var ang = terrain.RotationWithMouseHorizontally(Math.Max(1, multiplier.Round())) * 0.05f;

            terrain.TiltAngle = -ang;

            var mod = modifier * multiplier;

            Angle.X += ang * mod;
            Angle.Z += ang * mod;

            base.OnFrame(multiplier);
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
