using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ExtendRacketPill : PillBase
    {
        internal ExtendRacketPill(Vector3F location) : base(location, "E_ExtendRacket.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().ExtendRacketBallPillPicked();
        }
    }
}
