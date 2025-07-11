using Kriptok.Games.BlastemUp.Common;
using Kriptok.Games.BlastemUp.Player;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public abstract class EnemyShotBase : EntityBase<ISpriteView>
    {
        protected float ShotAngle;
        private ISingleCollisionQuery<ShieldBall> shieldCollision;

        /// <summary>
        /// Consulta para saber si salió de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        public EnemyShotBase(ISpriteView view, float x, float y, float angle) : base(view)
        {
            Location.X = x;
            Location.Y = y;
            ShotAngle = angle;

            Location.Z = 1;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision2DEllipse();
            this.shieldCollision = h.GetCollision2D<ShieldBall>();
            this.outOfScreen = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                Die();
                return;
            }

            Advance();
            
            if (shieldCollision.OnCollision(out ShieldBall ball))
            {
                // Genera una explosion y acaba este proceso
                if (IsAlive())
                {
                    Add(new Explosion(Rand.Next(0, 2), Rand.Next(0, 19) + 30, Location.X, Location.Y));
                }
#if DEBUG
                else
                {
                }
#endif

                Die();
                return;
            }
        }

        internal abstract void Advance();
    }

    public abstract class EnemyShotBase<T> : EnemyShotBase where T : ISpriteView
    {
        protected EnemyShotBase(ISpriteView view, float x, float y, float angle) 
            : base(view, x, y, angle)
        {            
        }

        /// <summary>
        /// Propiedad que indica el gráfico que debe renderizar asociado a este proceso.
        /// </summary>        
        public new T View => (T)base.View;
    }
}
