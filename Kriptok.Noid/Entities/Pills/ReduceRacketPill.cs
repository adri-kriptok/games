using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ReduceRacketPill : PillBase
    {
        internal ReduceRacketPill(Vector3F location) : base(location, "R_ReduceRacket.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().ReduceRacketBallPillPicked();
        }
    }
}
