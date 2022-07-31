using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ReduceRacketPill : PillBase
    {
        internal ReduceRacketPill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            Find.First<Racket>().ReduceRacketBallPillPicked();
        }
    }
}
