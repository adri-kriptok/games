using Kriptok.Entities.Base;
using System;
using Kriptok.Views.Primitives;
using Kriptok.Helpers;
using Kriptok.Drawing;
using System.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Scenes;

namespace Kriptok.Asteroids.Entities
{
    class Piece : EntityBase<PolygonView>
    {
        /// <summary>
        /// Angulo aleatorio.        
        /// </summary>
        public float angle;

        /// <summary>
        /// Velocidad de giro.
        /// </summary>
        public float incrementAngle;

        public Piece(Vector3F location, PointF scale, float angle, PolygonView poly) : base(poly)
        {
            Location = location;
            View.Scale = scale;
            Angle.Z = angle;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);        

            // Reinicia variables al azar para los angulos.
            angle = Rand.NextF(0f, 2f * MathHelper.PIF);
            incrementAngle = Rand.NextF(-MathHelper.PIF / 32f, MathHelper.PIF / 32f);
        }

        protected override void OnFrame()
        {
            // Gira los gráficos.
            Angle.Z += incrementAngle;

            // Los mueve.            
            XAdvance2D(2f, angle);

            // Y los re-escala (cada vez mas pequeños).
            View.ScaleX -= 0.02f;
            View.ScaleY -= 0.02f;

            if (View.Scale.X <= 0f)
            {
                // Actualiza la variable para indicar que has muerto
                Global.Dead = true;                       

                Die();
                return;
            }
        }
    }
}
