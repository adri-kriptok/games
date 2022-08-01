using Kriptok.Objects.Base;
using System.Drawing;
using Kriptok.Views.Primitives;
using Kriptok.Objects.Collisions;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Scenes;
using Kriptok.Objects.Queries;
using Kriptok.Objects.Queries.Base;

namespace Kriptok.Asteroids.Objects
{
    class Shot : ObjectBase<RectangleView>
    {
        private IQuery<bool> outOfScreen;

        public Shot(Vector3F location, float angle) : base(new RectangleView(16, 4, Color.White))
        {
            Location = location;
            Angle.Z = angle;            
        }

        protected override void OnStart(ObjectStartHandler h)
        {                    
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.outOfScreen = h.GetOutOfScreenQuery();

            Advance2D(16); 
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result)
            {
                Die();
                return;
            }

            Advance2D(16);
        }
    }
}
