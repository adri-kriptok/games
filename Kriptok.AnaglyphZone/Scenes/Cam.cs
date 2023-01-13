using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.AZ.Entities;

namespace Kriptok.AZ.Scenes
{
    internal class Cam : Pseudo3DCustomizerCamera
    {
        private readonly PlayerShip player;

        public Cam(IPseudo3DTarget target, PlayerShip player) : base(target, 300f)
        {
            this.player = player;
        }

        public override float GetCameraHeight() => AZConsts.CamHeight + player.RelativeLocation.Y * 0.5f;

        public override float GetDirection() => -MathHelper.HalfPIF * 7 / 8f;
    }

    internal abstract class CamTargetBase : EntityBase
    {
        public CamTargetBase()
        {            
            Location.Z = AZConsts.CamHeight;
        }

        protected override void OnFrame()
        {
            Location.X += 5f;
        }
    }
}
