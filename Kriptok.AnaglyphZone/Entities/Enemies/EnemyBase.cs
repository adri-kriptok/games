using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.AZ.Scenes;
using Kriptok.Views.Shapes;
using System;

namespace Kriptok.AZ.Entities.Enemies
{
    internal abstract class EnemyBase : EntityBase<GdipShapeView>
    {
        private readonly CamTargetBase camTarget;

        /// <summary>
        /// Vida que le queda al enemigo.
        /// </summary>
        protected int Life;

        public EnemyBase(CamTargetBase camTarget, GdipShapeView view) : base(view)
        {
            this.camTarget = camTarget;
            Location.X = camTarget.Location.X + 500;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();
        }

        protected override void OnFrame()
        {
            if ((camTarget.Location.X - Location.X) > 700)
            {
                Die();
            }

            if (Life <= 0)
            {
                Add(new Explosion2(Location));

                // Incremento la puntuación.
                var score = (int)(View.Scale.X + View.Scale.Y + View.Scale.Z);
                Global.Score += score;
                Add(new EnemyScore(Location, score));
                Die();
            }
        }

        internal void Damage(int v)
        {
            Life -= v;
        }
    }
}