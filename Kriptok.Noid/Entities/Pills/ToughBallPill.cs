using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ToughBallPill : PillBase
    {
        internal ToughBallPill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            foreach (var ball in Find.All<Ball>())
            {
                ball.SuperBallPicked();
            }
        }
    }
}
