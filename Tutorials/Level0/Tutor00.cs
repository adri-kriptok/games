using Kriptok;
using Kriptok.Audio;
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
using System;
using System.Drawing;

namespace Tutorials.Level0
{
    static partial class Tutor00
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
            Engine.Start(new InitScene(), p =>
            {
                p.FullScreen();
                p.Mode = WindowSizeEnum.W800x450;
                p.Title = "Tutor - 0 - 0 | Kriptok";
                p.CaptureMouse();
            });
        }

        public class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
                h.ScreenRegion.SetBackground(bg => bg.FillStretched(Assembly, "Level0.Assets.Images.Background.png"));

                h.Add(new Ship());

                h.Add(new AsteroidCreator(h.ScreenRegion.Size.Width - 100));
            }
        }

        internal class AsteroidCreator : EntityBase
        {
            private readonly int maxX;

            public AsteroidCreator(int maxX)
            {
                this.maxX = maxX;
            }            

            protected override void OnFrame()
            {
                if (Rand.Next(1, 10) > 7)
                {
                    Add(new Asteroid(Rand.Next(100, maxX), Rand.Next(-3, 3), Rand.NextF(0.5f, 1f)));
                }
            }
        }

        public class Ship : EntityBase<SpriteView>
        {
            private int shootCounter;
            private ISoundHandler shootSound;

            public Ship() : base(new SpriteView(DivResources.Image("Tutorial.Spaceship.png")))
            {
                Location.Y = 400;
                Angle.Z = (float)(-Math.PI / 2);
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                Location.X = h.RegionSize.Width / 2;

                shootSound = h.Audio.GetSoundHandler("Assets.Sounds.Shot1.wav");
            }

            protected override void OnFrame()
            {
                var pos = Mouse.X;

                if (pos < 20)
                {
                    Location.X = 20;
                }
                else if (pos > 780)
                {
                    Location.X = 780;
                }
                else
                {
                    Location.X = pos;
                }

                if (Mouse.Left)
                {
                    if (shootCounter <= 0)
                    {
                        shootSound.Play();
                        shootCounter = 5;
                    }

                    Add(new Shot(Location.X));
                }

                if (shootCounter >= 0)
                {
                    shootCounter -= 1;
                }
            }
        }

        public class Shot : EntityBase<SpriteView>
        {
#if DEBUG
            private IQuery<bool> outOfScreen;
#endif

            public Shot(float x) : base(new SpriteView(DivResources.Image("Tutorial.Shot.png")))
            {
                Location.X = x;
                Location.Y = 350;
                Angle.Z = (float)(-Math.PI / 2);

            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Auto;
#if DEBUG
                outOfScreen = h.GetOutOfScreenQuery();
#endif
            }

            protected override void OnFrame()
            {
                for (int i = 0; i < 30; i++)
                {
                    Location.Y -= 1;
                }

                // Es mucho más performante validar por coordenada Y.
                // Pero utilizar la query de "salir de la pantalla" en modo debug, ayuda a testear el motor.
#if DEBUG
                if (outOfScreen.Result)
#else
                if (Location.Y < 0)
#endif
                {
                    Die();
                }
            }
        }

        public class Asteroid : EntityBase<IndexedSpriteView>
        {
            private readonly int modX;
            private readonly int speed;

            /// <summary>
            /// Contador utilizado para saber cuándo cambiar el gráfico.
            /// </summary>
            private int changeGraphCounter = 0;

            /// <summary>
            /// Consulta para resolver colisiones con los disparos del protagonista.
            /// </summary>
            private ISingleCollisionQuery<Shot> shotCollision;

            /// <summary>
            /// Utilizado para evaluar si el objeto se encuentra fuera de la pantalla.
            /// </summary>
            private IQuery<bool?> outOfScreen;

            public Asteroid(int x, int modX, float size)
                : base(new IndexedSpriteView(DivResources.Image($"Tutorial.Asteroid0.png"), 5, 4))
            {
                Location.X = x;
                Location.Y = 0;
                this.modX = modX;
                speed = Rand.Next(2, 5);

                View.Scale = new PointF(size, size);
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Auto;
                shotCollision = h.GetCollision2D<Shot>();
                outOfScreen = h.GetOutOfScreenQuery();
            }

            protected override void OnFrame()
            {
                if (changeGraphCounter++ % 2 == 0)
                {
                    View.Rotate();
                }

                if (outOfScreen.Result.GetValueOrDefault(false))
                {
                    Die();
                    return;
                }

                Location.X += modX;
                Location.Y += speed;
                Angle.Z += MathHelper.CosF(speed) * 0.1f;

                if (shotCollision.OnCollision(out Shot coll))
                {
                    Add(new Explosion(Location, View.Scale, Angle.Z));

                    coll.Die();

                    Die();
                }
            }
        }
    }
}
