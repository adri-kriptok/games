﻿using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Views.Shapes;
using static Kriptok.Tehuelche.Enemies.EnemyMissile;

namespace Kriptok.Tehuelche.Enemies
{
    internal class EnemyMissile : EntityBase<EnemyMissileView>
    {
        private readonly EnemyBase owner;
        private readonly ITerrain terrain;
        private ISingleCollisionQuery<PlayerHelicopterBase> playerCollision;

        /// <summary>
        /// Sonidos del misil enemigo.
        /// </summary>
        private ISoundHandler explosi6Sound, explosi8Sound;

        public EnemyMissile(EnemyBase owner, ITerrain terrain, Vector3F location, float angleZ, float angleY)
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

            playerCollision = h.GetCollision3D<PlayerHelicopterBase>();

            explosi6Sound = h.Audio.GetWaveHandler(DivResources.Sound("Guerra.EXPLOSI6.WAV"));
            explosi8Sound = h.Audio.GetWaveHandler(DivResources.Sound("Guerra.EXPLOSI8.WAV"));
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

            if (playerCollision.OnCollision(out PlayerHelicopterBase player))
            {                
                explosi8Sound.Play();
                player.Hit();
                Explode();
                return;
            }

            Add(new SmokeParticle(Location));

            Advance3D(0.1875f * Sys.TimeDelta);

            // Me fijo si no choqué contra el piso.
            if (terrain.GetHeight(Location.XY()) >= Location.Z)
            {                
                explosi6Sound.Play();
                Explode();
                return;
            }

            void Explode()
            {                
                Add(new Explosion(Location, 1f));
                Die();
            }
        }

        internal class EnemyMissileView : MqoMeshView
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