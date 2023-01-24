using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Views.Base;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System.Drawing;

namespace Kriptok.Tehuelche.Entities.Player
{
    internal class Blank : EntityBase<RectangleView>
    {
        public Blank() : base(new BlankView())
        {            
        }

        internal EnemyBase Enemy;

        protected override void OnFrame()
        {            
        }

        public override Vector3F GetRenderLocation()
        {
            if (Enemy != null)
            {
                return Enemy.GetRenderLocation();
            }

            return base.GetRenderLocation();
        }

        private class BlankView : RectangleView
        {
            private Blank owner;

            public BlankView() : base(15, 15, null, Strokes.Green)
            {
            }

            public override void SetOwner(IViewOwner entity)
            {
                base.SetOwner(entity);
                this.owner = (Blank)entity;
            }

            protected override bool IsVisible()
            {
                if (owner.Enemy == null)
                {
                    return false;
                }
                return base.IsVisible();
            }

            public override float GetPriority() => base.GetPriority() + 999999f;            
        }
    }
}