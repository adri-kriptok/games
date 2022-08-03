using Kriptok.Entities.Base;
using System.Drawing;
using Kriptok.Views.Primitives;
using Kriptok.Entities.Collisions;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Scenes;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;

namespace Kriptok.Asteroids.Entities
{
    class Shot : EntityBase<RectangleView>
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
