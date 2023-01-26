using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Views;
using static Kriptok.Tehuelche.Entities.PlayerMissile;

namespace Kriptok.Tehuelche.Entities
{
    internal partial class PlayerMissile : EntityBase<PlayerMissileView>
    {
        /// <summary>
        /// Para alternar de qué lado del helicóptero salen los misiles.
        /// </summary>
        private static float side = 1.5f;

        private readonly PlayerHelicopter player;
        private readonly TehuelcheMapRegion terrain;
        private ISingleCollisionQuery<EnemyBase> enemyCollision;
        private ISoundHandler explosi6Sound;

        public PlayerMissile(PlayerHelicopter player, TehuelcheMapRegion terrain)
            : base(new PlayerMissileView())
        {
            this.player = player;
            this.terrain = terrain;
            
            Location = player.GetShootingLocation();
            Angle = player.GetShootingDirection();
        }

        public PlayerMissile(PlayerHelicopter player, TehuelcheMapRegion terrain, EnemyBase enemy)
            : base(new PlayerMissileView())
        {
            this.player = player;
            this.terrain = terrain;
            
            Location = player.Location;            
            LookAt3D(enemy.Location);
        }        

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Strafe2D(side = -side);

            h.SetCollision3DVertex();

            enemyCollision = h.GetCollision3D<EnemyBase>();

            this.explosi6Sound = h.Audio.GetWaveHandler(DivResources.Sound("Guerra.EXPLOSI6.WAV"));
        }

        protected override void OnFrame()
        {
            // Si me alejo demasiado del jugador...
            if (GetDistance3D(player) > 400f)
            {
                // ... listo, desaparece.
                Die();
                return;
            }

            if (enemyCollision.OnCollision())
            {
                Explode();
                return;
            }

            Add(new SmokeParticle(Location));

            Advance3D(0.1875f * Sys.TimeDelta);

            // Me fijo si no choqué contra el piso.
            if (terrain.SampleHeight(Location.XY()) >= Location.Z)
            {
                Explode();
                return;
            }

            void Explode()
            {                
                explosi6Sound.Play();
                Add(new PlayerMissileExplosion(Location, 1f));
                Die();
            }
        }

        internal class PlayerMissileView : VoxelSpaceShapeViewBase
        {
            public PlayerMissileView() 
                : base(typeof(PlayerMissileView).Assembly, "Assets.Models.Missile.mqo")
            {
                ScaleX = 0.05f;
                ScaleY = 0.05f;
                ScaleZ = 0.05f;
            }
        }
    }
}