using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System.Drawing;

namespace Kriptok.Tehuelche.Entities.Hud
{
    internal class MinimapEnemy : EntityBase<EllipseView>
    {
        private readonly EntityBase owner;

        public MinimapEnemy(EntityBase owner)
            : base(new EllipseView(2, 2, new FillConfig(Color.DarkRed), Strokes.Red))
        {
            this.owner = owner;
        }

        protected override void OnFrame()
        {
            if (!owner.IsAlive())
            {
                Die();
            }
        }

        public override Vector3F GetRenderLocation()
        {
            return owner.GetRenderLocation();
        }
    }
}