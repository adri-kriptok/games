using Kriptok.Extensions;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;

namespace Galax
{
    public abstract class EnemyShotBase<T> : EnemyShotBase
        where T : ISpriteView
    {
        private IQuery<bool?> outOfScreen;

        protected EnemyShotBase(T view, float x, float y) : base(view, x, y)
        {
        }

        /// <summary>
        /// Obtiene la vista asociada al objeto.
        /// </summary>
        public new T View => (T)base.View;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            outOfScreen = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                Die();
                return;
            }

            base.OnFrame();
        }
    }

    public abstract class EnemyShotBase : EntityBase
    {
        /// <summary>
        /// Porcentaje horizontal para usar con coordenada y.
        /// </summary>
        private float xAdvance;

        public EnemyShotBase(IView view, float x, float y) : base(view)
        {
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;

            h.Audio.GetSoundHandler("LASER2.WAV").Play();

            if (Global.Player.Location.Y == Location.Y)
            {
                // Si la nave esta en la vertical el incremento horizontal es 0
                xAdvance = 0f;
            }
            else
            {
                xAdvance = ((Global.Player.Location.X - Location.X) * 3 / (Global.Player.Location.Y - Location.Y) / 100f).Clamp(-100f, 100f);
            }
        }

        protected override void OnFrame()
        {
            Animate();

            // Mueve al proceso
            Location.Y += 3f;
            Location.X += xAdvance;
        }

        protected virtual void Animate()
        {
        }
    }
}