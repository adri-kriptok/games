using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Regions.Base;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Tehuelche.Entities;
using System;

namespace Kriptok.Tehuelche.Regions
{
    internal class PlayerCam : TargetPseudo3DCameraBase    
    {
        private const float camModifierVert = 3.25f;
        internal const float CamModifier = 60f;
        private const float thirdPersonCameraDistance = 17.5f;
        private const float inverseThirdPersonCameraDistance = 1f / thirdPersonCameraDistance;
        private readonly PlayerHelicopterPseudo3D heli;
        private readonly AutoShearPseudo3DCamera thirdPersonCamera;
        private readonly Pseudo3DWithMouseLookCamera2 firstPersonCamera;

        public PlayerCam(PlayerHelicopterPseudo3D heli) : base(heli)
        {
            this.heli = heli;

            thirdPersonCamera = new AutoShearPseudo3DCamera(heli);
            firstPersonCamera = new Pseudo3DWithMouseLookCamera2(heli);
            //Height = 0f;
            // Distance = 17.5f;
            // YShearing = -70f;
        }

        public override Vector2F GetLocation2D()
        {
            return base.GetLocation2D().Plus(heli.GetRandomShake());
        }

        public override float GetDistance()
        {
            var distance = heli.UserFirstPersonCamera ? 0f : thirdPersonCameraDistance;

            var diff = distance - Distance;
            if (diff != 0f)
            {
                if (Math.Abs(diff).Round1() <= 0f)
                {
                    Distance = distance;
                }
                else
                {
                    Distance += diff * 0.33f;
                }
            }

            return base.GetDistance();
        }

        /// <inheritdoc/>
        public override float GetDirection() => heli.CameraAngle;

        public override float GetCameraHeight()
        {
            if (Distance == 0f)
            {
                return heli.Location.Z;
            }
            else 
            {
                var verticalAngle = firstPersonCamera.GetVerticalAngle() - 0.3f;
                var height = (float)Math.Sin(verticalAngle) * Distance + camModifierVert;

                if (Distance == thirdPersonCameraDistance)
                {
                    return heli.Location.Z + height;
                }
                else
                {                    
                    var percentage = Distance * inverseThirdPersonCameraDistance;

                    return heli.Location.Z + percentage * height;
                }
            }
        }
       

        public override float GetYShearing(Pseudo3DRenderContext context, float tiltSin, float tiltCos)
        {
            if (Distance == 0f)
            {
                return GetFPShearing();
            }
            else if (Distance == thirdPersonCameraDistance)
            {
                return GetTPShearing();
            }
            else
            {
                var perc1 = Distance * inverseThirdPersonCameraDistance;
                var perc2 = 1f - perc1;

                var s1 = GetTPShearing();
                var s2 = GetFPShearing();

                return s1 * perc1 + s2 * perc2;
            }

            float GetFPShearing() => firstPersonCamera.GetYShearing(context, tiltSin, tiltCos);            

            // float GetTPShearing() => thirdPersonCamera.GetYShearing(context, tiltSin, tiltCos) * 0.5f - camModifier * 0.75f - 20f;
            float GetTPShearing() => thirdPersonCamera.GetYShearing(context, tiltSin, tiltCos) * 0.5f - CamModifier * 0.75f - 10f;
        }

        internal float GetVerticalAngle()
        {
            return firstPersonCamera.GetVerticalAngle();
        }

        private class Pseudo3DWithMouseLookCamera2 : Pseudo3DWithMouseLookCamera
        {
            public Pseudo3DWithMouseLookCamera2(EntityBase target) : base(target)
            {
            }

            protected override float GetYShearing(float inverseFov)
            {
                return base.GetYShearing(inverseFov) - CamModifier;
            }
        }
    }
}
