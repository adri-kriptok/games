using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Helpers;
using Kriptok.AZ.Entities.Enemies;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System.Drawing;
using System.Security.Cryptography;

namespace Kriptok.AZ.Entities
{
    internal class PlayerShot : EntityBase
    {
        private static readonly PointF[] vertices = new PointF[4]
        {
            new PointF(15f, 0f),
            new PointF(20f, 5f),
            new PointF(15f, 10f),
            new PointF(0f, 5f)
        };
        
        private readonly PlayerShip owner;
        // private ISingleCollisionQuery<Beam> beamCollision;
        // private ISingleCollisionQuery<Column> columnCollision;
        private ISingleCollisionQuery<EnemyBase> enemyCollision;

        public PlayerShot(PlayerShip owner, float yMod, float zMod) 
            : base(new PolygonView(vertices, Color.Fuchsia))
            //: base(new FlatCubeView(Material.Fuchsia)
            // {
            //     ScaleX = 25f,
            //     ScaleY = 25f,
            //     ScaleZ = 25f
            // })
        {
            this.owner = owner;
            // Location.Y = owner.Location.Y;
            // Location.Z = owner.Location.Z;
            Location.X = owner.Location.X + 80f;

            var sin = MathHelper.SinF(-owner.Angle.X);
            var cos = MathHelper.CosF(-owner.Angle.X);

            Location.Y = owner.Location.Y + yMod * cos + zMod * sin;
            Location.Z = owner.Location.Z + yMod * sin + zMod * cos;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Radius = 10;
            h.SetCollision3DSphere();

            // beamCollision = h.GetCollision3D<Beam>();
            // columnCollision = h.GetCollision3D<Column>();
            enemyCollision = h.GetCollision3D<EnemyBase>();
        }

        protected override void OnFrame()
        {
            // if (beamCollision.OnCollision())
            // {
            //     Add(new Explosion(Location));                
            //     Die();
            //     return;
            // }

            //if (columnCollision.OnCollision())
            //{
            //    Add(new Explosion1(Location));
            //    Die();
            //    return;
            //}

            if (enemyCollision.OnCollision(out EnemyBase enemy))
            {                
                Add(new Explosion(new Vector3F(Location.X, enemy.Location.Y + 1f, Location.Z)));
                enemy.Damage(1);
                
                Die();
                return;
            }

            Location.X += 40f;

            if (Location.X - owner.Location.X > 600)
            {
                Die();
                return;
            }            
         }
    }
}