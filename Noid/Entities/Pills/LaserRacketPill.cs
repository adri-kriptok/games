using Kriptok.Drawing.Algebra;

namespace Noid.Entities.Pills
{
    internal class LaserRacketPill : PillBase
    {
        internal LaserRacketPill(Vector3F location) : base(location, "L_Laser.png")
        {
        }

        protected override void OnPick()
        {
            Find.First<Racket>().LaserPillPicked();
        }
    }
}
