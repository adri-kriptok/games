using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class BackwardsPill : PillBase
    {
        internal BackwardsPill(Vector3F location) : base(location, "B_Backwards.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().BackwardsPillPicked();
        }
    }
}
