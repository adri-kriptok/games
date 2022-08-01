using Kriptok.Objects.Collisions;
using Kriptok.Objects.Base;
using Kriptok.Views.Primitives;
using System.Drawing;
using Kriptok.Drawing;
using Kriptok.Views.Sprites;

namespace Kriptok.Snake.Objects
{
    /// <summary>    
    /// Maneja los segmentos del cuerpo del gusano.
    /// </summary>
    class SnakeSegment : ProcessBase<SpriteView>
    {
        readonly int n;
        readonly int priority;
        private SnakeSegment son;

        public SnakeSegment(int n, int priority) : base(new SpriteView(typeof(SnakeHead).Assembly, "Red.png"))
        {
            this.n = n;
            this.priority = priority;

            Location.X = -20;
            Location.Y = -20;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Radius;
        }

        protected override void OnBegin()
        {
            // Si no es el ultimo cuerpo crea otro con mayor prioridad
            // y menor n£mero de cuerpo
            if (n > 0)
            {
                son = Add(new SnakeSegment(n - 1, priority + 1));
            }

            Loop(() =>
            {
                // Comprueba la prioridad que indica el orden en la cola
                // si esta dentro de la longitud de la cola
                if (priority < Global.SnakeLength)
                {
                    // Si esta dentro de la longitud lo imprime                    
                    View.ScaleX = 1f;
                    View.ScaleY = 1f;
                }
                else
                {                    
                    // Sino, lo saco de la pantalla.
                    Location.X = -9999f;
                    Location.Y = -9999f;
                }

                Frame();
            });
        }

        internal void SetXY(float x, float y)
        {
            if (son != null)
            {
                son.SetXY(Location.X, Location.Y);
            }

            Location.X = x;
            Location.Y = y;
        }

        public void Remove()
        {
            if (son != null)
            {
                son.Remove();
            }

            Location.X = -20f;
            Location.Y = -20f;
        }
    }

}
