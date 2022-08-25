using Kriptok;
using Kriptok.Core;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Scenes;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tutorials.Level0
{
    static class Tutor01
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Config.Get<BaseConfiguration>().Mute();
#endif
            Engine.Start(new InitScene(), s => 
            {
                s.FullScreen = true;                
                s.Mode = WindowSizeEnum.W800x450;
                s.Title = "Tutor - 0 - 1 | Kriptok";
            });
        }

        public class InitScene : SceneBase
        {
            internal static int XBound;
            internal static int XInc;
            internal static int YBound;
            internal static int YInc;

            protected override void Run(SceneHandler h)
            {
                h.ScreenRegion.SetBackground(bg => bg.FillStretched(Assembly, "Level0.Assets.Images.Background.png"));

                XBound = h.ScreenRegion.Size.Width + 20;
                YBound = h.ScreenRegion.Size.Height + 20;

                XInc = XBound + 40;
                YInc = YBound + 40;

                var ship = h.Add(new Ship());

                var font = new SuperFont(new Font("Quartz MS", 20), Color.White, Color.Cyan)
                    .SetShadow(2, 2, Color.Blue);

                h.Write(font, h.ScreenRegion.Size.Width / 2, 15, () => ship.Points.ToString("000000"));

                for (int i = 0; i < 4; i++)
                {
                    h.Add(new Asteroid(ship, 0));
                    h.Add(new Asteroid(ship, 1));
                }
            }
        }

        /// <summary>
        /// Rocas a destruir por la nave protagonista.
        /// </summary>
        public class Asteroid : RelocalizableBase<IndexedSpriteView>
        {
            private float angle;

            /// <summary>
            /// Nave principal.
            /// </summary>
            private readonly Ship ship;

            /// <summary>
            /// Velocidad a la que se mueve el asteroide.
            /// </summary>
            private int speed;

            /// <summary>
            /// Contador utilizado para saber cuándo cambiar el gráfico.
            /// </summary>
            private int changeGraphCounter = 0;

            /// <summary>
            /// Consulta de colisión contra un disparo.
            /// </summary>
            private ISingleCollisionQuery<Shot> shotCollision;

            /// <summary>
            /// Genera una nueva instancia de la clase <see cref="Asteroid"/>
            /// </summary>
            /// <param name="ship">Recibe la nave protagonista por parámetro para poder sumarle los puntos.</param>
            /// <param name="type">Tipo de asteroide.</param>        
            public Asteroid(Ship ship, int type)
                : base(new IndexedSpriteView(DivResources.Image($"Tutorial.Asteroid{type}.png"), 5, 4))
            {
                Reset();

                this.ship = ship;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Auto;
                this.shotCollision = h.GetCollision2D<Shot>();
            }

            private void Reset()
            {
                Location.X = 0;
                Location.Y = 0;

                angle = Rand.NextF(MathHelper.TwoPIF);
                speed = Rand.Next(1, 3);
            }

            protected override void OnFrame()
            {
                if (changeGraphCounter++ % 2 == 0)
                {
                    View.Rotate();
                }

                Angle.Z += MathHelper.CosF(speed) * 0.005f;
                XAdvance2D(speed, angle);

                if (shotCollision.OnCollision(out Shot coll))
                {
                    // Le indico al disparo que lo tocó que muera.
                    coll.Die();

                    // Muestro la explosión.
                    Add(new Explosion(Location, View.Scale, Angle.Z));

                    // Y finalmente, sumo puntos.
                    ship.Points += 100;

                    // Reseteo el objeto.
                    Reset();
                }

                Relocate();
            }
        }

        public class Ship : RelocalizableBase<SpriteView>
        {
            private const float rotation = (float)(Math.PI / 32);
            public int Points;
            private int shootCounter;

            public Ship() : base(new SpriteView(DivResources.Image("Tutorial.Spaceship.png")))
            {
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                Location.X = h.RegionSize.Width / 2;
                Location.Y = h.RegionSize.Height / 2;
            }

            protected override void OnFrame()
            {
                if (Input.Left())
                {
                    Angle.Z -= rotation;
                }

                if (Input.Right())
                {
                    Angle.Z += rotation;
                }

                if (Input.Up())
                {
                    Advance2D(5);
                }

                if (Input.Down())
                {
                    Advance2D(-5);
                }

                if (Input.Key(Keys.Space))
                {
                    if (shootCounter <= 0)
                    {
                        Audio.PlaySound(GetType().Assembly, "Assets.Sounds.Shot1.wav");
                        shootCounter = 5;
                    }

                    Add(new Shot(Location.X, Location.Y, Angle.Z));
                }

                if (shootCounter >= 0)
                {
                    shootCounter -= 1;
                }

                Relocate();
            }
        }

        public class Shot : EntityBase<SpriteView>
        {
            /// <summary>
            /// Consulta para ver si salió de la pantalla.
            /// </summary>
            private IQuery<bool> outOfScreen;

            public Shot(float x, float y, float angle) : base(new SpriteView(DivResources.Image("Tutorial.Shot.png")))
            {
                Location.X = x;
                Location.Y = y;

                Angle.Z = angle;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Rectangle;
                outOfScreen = h.GetOutOfScreenQuery();

                Advance2D(50);                
            }

            protected override void OnFrame()
            {
                if (outOfScreen.Result)
                {
                    Die();
                    return;
                }

                for (int i = 0; i < 4; i++)
                {
                    Advance2D(4);
                }
            }
        }

        public abstract class RelocalizableBase<T> : EntityBase<T> where T : IView
        {
            protected RelocalizableBase(T view) : base(view)
            {
            }

            internal void Relocate()
            {
                // Si se sale por la izquierda hace que aparezca por la derecha
                // restando para ello el ancho de pantalla
                if (Location.X < -20) Location.X += InitScene.XInc;
                // Si se sale por la derecha hace que aparezca por la izquierda
                if (Location.X > InitScene.XBound) Location.X -= InitScene.XInc;

                // Si se sale por la arriba hace que aparezca por la abajo
                if (Location.Y < -20) Location.Y += InitScene.YInc;
                // Si se sale por la abajo hace que aparezca por la arriba
                if (Location.Y > InitScene.YBound) Location.Y -= InitScene.YInc;
            }
        }
    }
}
