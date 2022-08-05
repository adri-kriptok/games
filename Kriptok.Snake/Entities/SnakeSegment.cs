using Kriptok.Entities.Collisions;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;
using Kriptok.Drawing;
using Kriptok.Views.Sprites;

namespace Snake.Entities
{
    /// <summary>    
    /// Maneja los segmentos del cuerpo del gusano.
    /// </summary>
    class SnakeSegment : EntityBase<SpriteView>
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

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
        
            h.CollisionType = Collision2DTypeEnum.Radius;

            // Si no es el ultimo cuerpo crea otro con mayor prioridad
            // y menor n£mero de cuerpo
            if (n > 0)
            {
                son = Add(new SnakeSegment(n - 1, priority + 1));
            }
        }

        protected override void OnFrame()
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

            Die();
        }
    }

}
