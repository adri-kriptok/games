using Kriptok.Entities.Collisions;
using Kriptok.IO;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;
using System.Windows.Forms;
using Kriptok.Views.Sprites;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Audio;

namespace Snake.Entities
{
    /// <summary>               
    /// Maneja la cabeza del gusano.
    /// </summary>
    public class SnakeHead : EntityBase
    {
        private int ix;
        private int iy;
        private SnakeSegment son;
        private Keys lastKey;
        private ISingleCollisionQuery<Apple> appleCollision;
        private ISingleCollisionQuery<SnakeSegment> segmentCollision;
        public const int Size = 8;
        public const int HalfSize = 4;
        public const float RealSize = Size;

        /// <summary>
        /// Sonido cuando se come una manzana.
        /// </summary>
        private ISoundHandler appleSound;

        public SnakeHead() : base(new SpriteView(typeof(SnakeHead).Assembly, "Red.png"))
        {
            Location.X = Size + HalfSize;
            Location.Y = 11 * Size + HalfSize;

            this.ix = Size;
            this.iy = 0;

            lastKey = Keys.Right;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            h.CollisionType = Collision2DTypeEnum.Radius;
            appleCollision = h.GetCollision2D<Apple>();
            segmentCollision = h.GetCollision2D<SnakeSegment>();

            // Y se crea un cuerpo del gusano que crea a los otros
            son = Add(new SnakeSegment(128, 1));

            appleSound = h.Audio.GetSoundHandler("Resources.Item2.wav");
        }

        protected override void OnFrame()
        {
            // Comprueba las teclas de los cursores y cambia los incrementos
            if (Input.Right() && lastKey != Keys.Left)
            {
                ix = Size; iy = 0;
                lastKey = Keys.Right;
            }
            else if (Input.Left() && lastKey != Keys.Right)
            {
                ix = -Size; iy = 0;
                lastKey = Keys.Left;
            }
            else if (Input.Down() && lastKey != Keys.Up)
            {
                ix = 0; iy = Size;
                lastKey = Keys.Down;
            }
            else if (Input.Up() && lastKey != Keys.Down)
            {
                ix = 0; iy = -Size;
                lastKey = Keys.Up;
            }

            son.SetXY(Location.X, Location.Y);

            if (appleCollision.OnCollision(out Apple apple))
            {
                // Elimina esa manzana
                apple.Die();

                Global.Apples--;          // Decrementa el contador de manzanas
                Global.SnakeLength += 4;  // Incrementa la cola del gusano
                Global.Score += 10;       // Suma 10 puntos a la puntuaci¢n                    

                // Audio.PlaySound(Assembly, "Resources.Item2.wav");
                appleSound.Play();
            }

            if (!InLimits() || segmentCollision.OnCollision())
            {
                Scene.SendMessage("Reset");
                Sleep();
                return;
            }

            // Mueve al gusano en la direccion deseada
            Location.X = Location.X + ix;
            Location.Y = Location.Y + iy;
        }

        /// <summary>
        /// Mata a todos los segmentos, y luego muere.
        /// </summary>
        internal void Kill()
        {
            // Elimina todos los segmentos del gusano.
            son.Remove();
            Die();
        }

        private bool InLimits()
        {
            return !(Location.X < 5f || Location.Y < 22f || Location.X > 315f || Location.Y > 195f);
        }
    }
}
