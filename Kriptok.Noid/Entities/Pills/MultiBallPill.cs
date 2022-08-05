using Kriptok.Drawing.Algebra;

namespace Noid.Entities.Pills
{
    internal class MultiBallPill : PillBase
    {
        internal MultiBallPill(Vector3F location) : base(location, "M_MultiBall.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().MultiBallPillPicked();
        }
    }
}
