using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Tehuelche.Entities.Enemies;
using System;
using System.Diagnostics;

namespace Kriptok.Tehuelche.Entities.Player
{
    internal class PlayerMissileExplosion : Explosion
    {
        private IMultipleCollisionQuery<EnemyBase> enemyCollisions;

        public PlayerMissileExplosion(Vector3F location, float scale) : base(location, scale)
        {            
            Radius = (ushort)(4f * scale).Round();
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DSphere();

            this.enemyCollisions = h.GetCollisions3D<EnemyBase>();
        }

        protected override void OnFrame()
        {
            // Sólo valido colisión en el primer frame;
            if (View.Graph == 0)
            {
                if (enemyCollisions.OnCollision(out EnemyBase[] enemies))
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.Damage(Math.Max(1f, 20f - GetDistance3D(enemy)));
                    }
                }
            }

            base.OnFrame();
        }
    }
}