using Kriptok.Drawing.Algebra;

namespace Noid.Entities.Pills
{
    internal class DecreaseBallSpeedPill : PillBase
    {
        internal DecreaseBallSpeedPill(Vector3F location) : base(location, "D_DecreaseSpeed.png")
        {
        }

        protected override void OnPick()
        {
            foreach (var ball in Find.All<Ball>())
            {
                ball.DecreaseSpeedPicked();
            }
        }
    }
}
