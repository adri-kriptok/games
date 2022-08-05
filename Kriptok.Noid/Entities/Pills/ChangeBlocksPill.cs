using Kriptok.Drawing.Algebra;

namespace Noid.Entities.Pills
{
    internal class ChangeBlocksPill : PillBase
    {
        internal ChangeBlocksPill(Vector3F location) : base(location, "C_ChangeBlocks.png")
        {
        }

        protected override void OnPick()
        {
            foreach (var brick in Find.All<BrickSolid>())
            {
                brick.Change();
            }
        }
    }
}
