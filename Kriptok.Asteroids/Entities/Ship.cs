using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Kriptok.Asteroids.Entities
{
    class Ship : EntityBase<ShipView>
    {
        /// <summary>
        /// Indica si puede disparar o no.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Indica si puede realizar un salto al hiper espacio o no.        
        /// </summary>
        private bool hyper = true;

        private Vector2F speed;

        /// <summary>
        /// Tamaño de la pantalla.
        /// </summary>
        private Size regionSize;

        public Ship() : base(new ShipView())
        {            
            View.ScaleX = 1.25f;
            View.ScaleY = 1.25f;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);        
            h.CollisionType = Collision2DTypeEnum.Auto;

            regionSize = h.RegionSize;
            Location.X = regionSize.Width / 2;
            Location.Y = regionSize.Height / 2;
        }

        protected override void OnFrame()
        {
            // Lee teclas y actualiza el  ngulo de la nave
            if (Input.Right()) Angle.Z += MathHelper.PIF / 16;
            if (Input.Left()) Angle.Z -= MathHelper.PIF / 16;

            if (Input.Up())
            {                      // Calcula el avance con formula
                speed.X += PolarVector.ProjectX(Angle.Z, 10f);
                speed.Y += PolarVector.ProjectY(Angle.Z, 10f);
            }

            if (Input.Down())
            {                      // Calcula el avance con formula
                speed.X -= PolarVector.ProjectX(Angle.Z, 10f);
                speed.Y -= PolarVector.ProjectY(Angle.Z, 10f);
            }

            Location.X += speed.X * 0.1f;
            Location.Y += speed.Y * 0.1f;

            // Comprueba si se ha salido de la pantalla y lo soluciona
            Location = Helpers.Relocate(Location, regionSize);

            // Comprueba la tecla de disparo
            if (Input.Button03() || Input.Button04())
            {
                // Y si se puede dispara
                if (canShoot)
                {
                    // Dispara, creando un proceso tipo disparo nave
#if !DEBUG
                    canShoot = false;
#endif
                    // Sonido de disparo
                    Audio.PlaySound(Assembly, "TUBO8.WAV");
                    Add(new Shot(Location, Angle.Z));
                }
            }
            else
            {
                // Hace que los disparos salgan de uno en uno.
                canShoot = true;
            }

            // Si se pulsa [ESCAPE] se sale del juego
            if (Input.Escape())
            {
                Scene.SendMessage(PlaySceneMessages.Escape);
                Die();
                return;
            }

            // Comprueba la tecla del hiperespacio
            if (Input.Key(Keys.H))
            {
                // Y si se puede se hace
                if (hyper)
                {
                    hyper = false;
                    Add(new HyperSpace(Location));

                    // Pon la nave en una posici¢n aleatoria
                    Location.X = Rand.Next(0, regionSize.Width);
                    Location.Y = Rand.Next(0, regionSize.Height);

                    speed.X = 0f;
                    speed.Y = 0f;
                }
            }
            else
            {
                // Hace que los hiperespacios salgan de uno en uno
                hyper = true;
            }
        }

        /// <summary>
        /// Destruye la nave en piezas.
        /// </summary>
        public void Explode()
        {
            if (IsAlive())
            {
                var views = View.Split();

                Add(new Piece(Location, View.Scale, Angle.Z, views[0]));
                Add(new Piece(Location, View.Scale, Angle.Z, views[1]));
                Add(new Piece(Location, View.Scale, Angle.Z, views[2]));
                Add(new Piece(Location, View.Scale, Angle.Z, views[3]));

                Scene.SendMessage(PlaySceneMessages.Dead);

                Die();
            }
        }
    }

    class ShipView : PolygonView
    {
        public ShipView() : base(new PointF[4]
        {
            new PointF(32, 10),
            new PointF(0, 0),
            new PointF(6, 10),
            new PointF(0, 20)
        }, null, Strokes.Get(Color.Cyan, 2f, LineJoin.Round))
        {
            Rounded = true;
        }
    }
}
