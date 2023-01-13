using Kriptok.AZ.Scenes;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Views.Shapes;

namespace Kriptok.AZ.Entities
{
    internal class GameOver : EntityBase<WireframeTextView>
    {
        private readonly CamTargetBase camTarget;

        public GameOver(CamTargetBase camTarget) 
            : base(new WireframeTextView(AZConsts.DefaultFont, "GAME OVER", false, Strokes.Fuchsia))
        {
            this.camTarget = camTarget;
            View.ScaleX = 3f;
            View.ScaleY = 3f;
            View.ScaleZ = 3f;
            Angle.Z = -MathHelper.HalfPIF;
        }

        protected override void OnFrame()
        {
            Location = camTarget.Location;            
        }
    }
}