using Kriptok.Drawing.Algebra;

namespace Kriptok.Noid.Entities.Pills
{
    internal class ChangeBlocksPill : PillBase
    {
        internal ChangeBlocksPill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            foreach (var brick in Find.All<BrickSolid>())
            {
                brick.Change();
            }
        }
    }
}
