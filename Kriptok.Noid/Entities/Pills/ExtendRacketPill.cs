using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ExtendRacketPill : PillBase
    {
        internal ExtendRacketPill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            Find.First<Racket>().ExtendRacketBallPillPicked();
        }
    }
}
