using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    internal class Laser : EntityBase<SpriteView>
    {
        private ISingleCollisionQuery<Brick> brickCollision;

        public Laser(float x, float y) : base(new SpriteView(typeof(Laser).Assembly, "Assets.Images.Laser.png"))
        {
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;
            brickCollision = h.GetCollision2D<Brick>();
        }

        protected override void OnFrame()
        {
            if (brickCollision.OnCollision(out Brick brick))
            {
                if (brick.CanBeHitByLasers())
                {
                    brick.Hit();
                    Die();
                    return;
                }
            }

            if (Location.Y <= 4f)
            {
                Die();
                return;
            }

            Location.Y -= 4f;
        }
    }
}
