using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System.Drawing;

namespace Billiard.Entities
{
    /// <summary>
    /// Proceso visor 
    /// Muestra donde ira la bola
    /// </summary>
    public class Aim : EntityBase<EllipseView>
    {
        /// <summary>
        /// Bandera. 1=impacto 0=no impacto
        /// </summary>
        private bool impact = false;

        /// <summary>
        /// Identificador de la bola
        /// </summary>
        private readonly int ballId;

        private readonly float angle;

        public Aim(Ball ball, float angle) : base(new EllipseView(28, 28, null, Strokes.Get(Color.CornflowerBlue)))
        {
            this.ballId = ball.Id;
            this.angle = angle;            
            Location.Z = -200;

            // También coge las coordenadas de la bola que es lanzada
            Location.X = ball.Location.X;
            Location.Y = ball.Location.Y;
            Radius = 15;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            do
            {
                // Mueve el visor un punto en el angulo deseado
                var incr = PolarVector.NewVector(angle, 1f);

                // La coloca en la nueva posicion
                Location.X += incr.X;
                Location.Y += incr.Y;

                // Testea limites de la mesa
                if ((Location.Y <= 104f && incr.Y < 0f) ||
                    (Location.Y >= 374f && incr.Y > 0f) ||
                    (Location.X <= 62f && incr.X < 0f) ||
                    (Location.X >= 579f && incr.X > 0f))
                {
                    break;
                }

                // Va comprobando si colisiona con cualquiera otra bola
                foreach (var ball in Radius2DCollisions<Ball>())
                {
                    if (ball.Id != ballId)
                    {
                        impact = true;
                        break;
                    }
                }
            } while (!impact); // Busca la posicion del visor hasta que colisiones.
        }

        private bool rendered = false;
        protected override void OnFrame()
        {
            if (rendered)
            {
                Die();
            }
            else
            {
                rendered = true;
            }
        }

        //protected override void OnBegin()
        //{
            
        //    // Ahora ya puede mostrar el grafico del visor
        //    Frame();            
        //}
    }
}
