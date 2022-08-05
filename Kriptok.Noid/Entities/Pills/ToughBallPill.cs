using Kriptok.Drawing.Algebra;

namespace Noid.Entities.Pills
{
    internal class ToughBallPill : PillBase
    {
        internal ToughBallPill(Vector3F location) : base(location, "T_SuperBall.png")
        {
        }

        protected override void OnPick()
        {
            foreach (var ball in Find.All<Ball>())
            {
                ball.SuperBallPicked();
            }
        }
    }
}
