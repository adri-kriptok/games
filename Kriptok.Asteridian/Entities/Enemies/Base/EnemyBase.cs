using Kriptok.Asteridian.Regions;
using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Queries.Base;
using Kriptok.Extensions;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Enemies.Base
{
    internal abstract class EnemyBase : EntityBase
    {
        /// <summary>
        /// Indica si salió de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreenQuery;

        /// <summary>
        /// Indica si entró a la pantalla.
        /// </summary>
        private bool isOnScreen = false;

        /// <summary>
        /// Vida actual del enemigo.
        /// </summary>
        private float health = 0f;

        // protected EnemyBase()
        // {
        //     Location.Z = GlobalConsts.ZLevel.EnemyInAir;
        // }

        public EnemyBase(float baseHealth, IView view) : base(view)
        {
            Location.Z = GlobalConsts.ZLevel.EnemyInAir;
            this.health = baseHealth;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.outOfScreenQuery = h.GetOutOfScreenQuery();
            health = ((IAsteridianScroll)h.Region).CalculateEnemyHealth(health);
        }

        protected override void OnFrame()
        {
            if (outOfScreenQuery.Result.HasValue)
            {
                if (isOnScreen)
                {
                    if (outOfScreenQuery.Result.Value)
                    {
                        Die();
                        return;
                    }
                }
                isOnScreen = !outOfScreenQuery.Result.Value;
            }
            
            if (health < 0f)
            {
                OnDying();
                Die();
                return;
            }
        }

        /// <summary>
        /// Método a ejecutar cuando el objeto muere.
        /// </summary>
        protected virtual void OnDying()
        {            
        }

        internal virtual void StartOnTop(float y)
        {            
            if (View is GdipRenderizableBase rect)
            {
                var height = rect.GetSize().Height;
  
                Location.Y = y + height * rect.Center.Y - height;
            }            
        }

        /// <summary>
        /// Recibe daño directo del arma.
        /// </summary>        
        internal virtual void Hit(float damage)
        {
            health -= damage;
        }
    }
}
