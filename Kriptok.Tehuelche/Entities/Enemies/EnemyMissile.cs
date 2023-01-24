using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Views;
using static Kriptok.Tehuelche.Enemies.EnemyMissile;
using static Kriptok.Tehuelche.Entities.PlayerMissile;

namespace Kriptok.Tehuelche.Enemies
{
    internal class EnemyMissile : EntityBase<EnemyMissileView>
    {
        private readonly EnemyBase owner;
        private readonly TehuelcheMapRegion terrain;
        private ISingleCollisionQuery<PlayerHelicopter> playerCollision;

        public EnemyMissile(EnemyBase owner, TehuelcheMapRegion terrain, Vector3F location, float angleZ, float angleY)
             : base(new EnemyMissileView())
        {
            this.owner = owner;
            this.terrain = terrain;
            this.Location = location;
            Angle.Z = angleZ;
            Angle.Y = angleY;               
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Radius = 10;
            h.SetCollision3DSphere();

            playerCollision = h.GetCollision3D<PlayerHelicopter>();
        }

        protected override void OnFrame()
        {
            // Si me alejo demasiado del jugador...
            if (GetDistance3D(owner) > 400f)
            {
                // ... listo, desaparece.
                Die();
                return;
            }

            if (playerCollision.OnCollision(out PlayerHelicopter player))
            {
                Audio.PlayWave(DivResources.Sound("Guerra.EXPLOSI8.WAV"));
                player.Hit();
                Explode();
                return;
            }

            Add(new SmokeParticle(Location));

            Advance3D(0.1875f * Sys.TimeDelta);

            // Me fijo si no choqué contra el piso.
            if (terrain.SampleHeight(Location.XY()) >= Location.Z)
            {
                Audio.PlayWave(DivResources.Sound("Guerra.EXPLOSI6.WAV"));
                Explode();
                return;
            }

            void Explode()
            {                
                Add(new Explosion(Location, 1f));
                Die();
            }
        }

        internal class EnemyMissileView : VoxelSpaceShapeViewBase
        {
            public EnemyMissileView()
                : base(typeof(EnemyMissileView).Assembly, "Assets.Models.Missile.mqo")
            {
                ScaleX = 0.05f;
                ScaleY = 0.05f;
                ScaleZ = 0.05f;
            }
        }
    }
}