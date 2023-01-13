using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Views.Shapes;

namespace Kriptok.AZ.Entities.Enemies
{
    internal class EnemyScore : EntityBase<WireframeTextView>
    {
        private int counter = 0;

        public EnemyScore(Vector3F location, int score) 
            : base(new WireframeTextView(AZConsts.DefaultFont, string.Format("+{0}", score), false, Strokes.Fuchsia))            
        {
            Location = location;

            View.ScaleX = 3f;
            View.ScaleY = 3f;
            View.ScaleZ = 3f;
            Angle.Z = -MathHelper.HalfPIF;
        }

        protected override void OnFrame()
        {
            Location.Y += 1f;
            if (counter++ >= 100)
            {
                Die();
            }
        }
    }
}