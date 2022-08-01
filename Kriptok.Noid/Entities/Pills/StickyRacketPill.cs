using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class StickyRacketPill : PillBase
    {
        internal StickyRacketPill(Vector3F location) : base(location, "S_StickyRacket.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().StickyRacketPillPicked();
        }
    }
}
