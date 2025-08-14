using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Player.Weapons.Base
{
    internal abstract class PlayerWeaponBase : EntityBase
    {
        /// <summary>
        /// Intervalo de tiempo entre un disparo y otro.
        /// </summary>
        private readonly float timeInterval;

        /// <summary>
        /// Referencia al jugador para ubicar los disparos.
        /// </summary>
        private readonly PlayerShip player;

        /// <summary>
        /// Contador de tiempo entre disparo y disparo.
        /// </summary>
        private float timeCounter = 0f;

        public PlayerWeaponBase(PlayerShip player, float timeInterval)
        {
            this.timeInterval = timeInterval;
            this.player = player;
        }

        /// <inheritdoc/>
        public override bool IsAlive() => base.IsAlive() && player.IsAlive();

        protected sealed override void OnFrame()
        {
            timeCounter += Sys2.TimeDelta;

            if (timeCounter >= timeInterval)
            {
                timeCounter -= timeInterval;

                if (ShooterKeyPressed())
                {
                    Shoot(player.Location);
                }
            }
        }

        protected abstract bool ShooterKeyPressed();

        protected abstract void Shoot(Vector3F playerLocation);
    }

    internal abstract class FrontWeaponBase : PlayerWeaponBase
    {
        private readonly PlayerShip player;

        protected FrontWeaponBase(PlayerShip player, float timeInterval) : base(player, timeInterval)
        {
            this.player = player;
        }

        protected sealed override bool ShooterKeyPressed() => Mouse.Left;

        /// <summary>
        /// Devuelve el siguiente nivel de poder de la misma arma.
        /// </summary>        
        internal FrontWeaponBase GetLevelUp()
        {
            var nextWeapon = LevelUp(player);
            if (nextWeapon.Equals(this))
            {
                return this;
            }
            else
            {
                Add(nextWeapon);
                Die();
                return nextWeapon;
            }
        }

        /// <summary>
        /// Devuelve el nivel anterior de poder de la misma arma.
        /// </summary>        
        internal FrontWeaponBase GetLevelDown()
        {
            var prevWeapon = LevelDown(player);
            if (prevWeapon.Equals(this))
            {
                return this;
            }
            else
            {
                Add(prevWeapon);
                Die();
                return prevWeapon;
            }
        }

        /// <summary>
        /// Devuelve el siguiente nivel de poder de la misma arma.
        /// </summary>        
        protected abstract FrontWeaponBase LevelUp(PlayerShip player);

        /// <summary>
        /// Devuelve el nivel anterior de poder de la misma arma.
        /// </summary>        
        protected abstract FrontWeaponBase LevelDown(PlayerShip player);

        /// <inheritdoc/>
        public override Vector3F GetRenderLocation() => player.GetRenderLocation();
    }

    internal abstract class RearWeaponBase : PlayerWeaponBase
    {
        private readonly PlayerShip player;

        protected RearWeaponBase(PlayerShip player, float timeInterval) : base(player, timeInterval)
        {
            this.player = player;
        }

        protected sealed override bool ShooterKeyPressed() => Mouse.Left;

        /// <summary>
        /// Devuelve el siguiente nivel de poder de la misma arma.
        /// </summary>        
        internal RearWeaponBase GetLevelUp()
        {
            var nextWeapon = LevelUp(player);
            if (nextWeapon.Equals(this))
            {
                return this;
            }
            else
            {
                Add(nextWeapon);
                Die();
                return nextWeapon;
            }
        }

        /// <summary>
        /// Devuelve el nivel anterior de poder de la misma arma.
        /// </summary>        
        internal RearWeaponBase GetLevelDown()
        {
            var prevWeapon = LevelDown(player);
            if (prevWeapon.Equals(this))
            {
                return this;
            }
            else
            {
                Add(prevWeapon);
                Die();
                return prevWeapon;
            }
        }

        /// <summary>
        /// Devuelve el siguiente nivel de poder de la misma arma.
        /// </summary>        
        protected abstract RearWeaponBase LevelUp(PlayerShip player);

        /// <summary>
        /// Devuelve el nivel anterior de poder de la misma arma.
        /// </summary>        
        protected abstract RearWeaponBase LevelDown(PlayerShip player);
    }
}
