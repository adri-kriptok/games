using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Entities.Weapons
{
    class ShotBase : EntityBase<SpriteView>
    {
#if DEBUG
        private const float shootSpeed = Jazz.RunSpeed;
#else
        private const float shootSpeed = Jazz.RunSpeed + 4;
#endif
        /// <summary>
        /// Consulta para analizar si el objeto se encuentra fuera de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        /// <summary>
        /// Incremento sobre la coordenada X de los disparos.
        /// </summary>
        private readonly float incX;

        public ShotBase(Vector3F location, SpriteView view, FlipEnum flip) : base(view)
        {
            Location = location;
            view.Flip = flip;

            if (flip == FlipEnum.None)
            {
                incX = shootSpeed;
            }
            else
            {
                incX = -shootSpeed;
            }
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision2DEllipse();

            outOfScreen = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                Die();
                return;
            }

            Location.X += incX;
        }
    }
}
