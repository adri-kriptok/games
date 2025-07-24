using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Player
{
    /// <summary>
    /// Clase base para todos los disparos del jugador, que no evalúa "salirse de la pantalla" para 
    /// saber cuándo morir.
    /// </summary>
    internal abstract class PlayerShotBaseBase : EntityBase
    {
        private ISingleCollisionQuery<EnemyBase> collisionQuery;        

        public PlayerShotBaseBase(IView view) : base(view)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = this.GetCollisionType();
            
            this.collisionQuery = h.GetCollision2D<EnemyBase>();
        }

        /// <summary>
        /// Permite customizar la caja de colisión.
        /// </summary>        
        protected virtual Collision2DTypeEnum GetCollisionType() => Collision2DTypeEnum.Auto;

        protected override void OnFrame()
        {
            if (collisionQuery.OnCollision(out EnemyBase enemy))
            {
                // Si golpeó un enemigo, también, muere, pero antes hace daño.
                Damage(enemy);
                Die();
            }
            else
            {
                this.Frame();
            }
        }

        /// <summary>
        /// Daña al enemigo con el que ha hecho colisión.
        /// </summary>        
        protected virtual void Damage(EnemyBase enemy)
        {
            enemy.Hit(GetDamage());
        }

        /// <summary>
        /// Para implementación estándar de daño, indica cuánto daño debe hacer al enemigo.
        /// </summary>        
        protected abstract float GetDamage();

        /// <summary>
        /// Comportamiento estándar del disparo.
        /// </summary>
        protected abstract void Frame();
    }

    /// <summary>
    /// Clase base para todos los disparos del jugador.
    /// </summary>
    internal abstract class PlayerShotBase : PlayerShotBaseBase
    {
        private IQuery<bool?> outOfScreen;

        public PlayerShotBase(IView view) : base(view)
        {
        }

        protected sealed override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.outOfScreen = h.GetOutOfScreenQuery();
        }
        

        protected sealed override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                // Salió de la pantalla, listo.
                Die();
            }
            else
            {
                base.OnFrame();
            }
        }
    }
}
