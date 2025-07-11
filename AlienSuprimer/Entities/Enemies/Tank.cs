using Kriptok.Audio;
using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Context.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Games.Alien.Entities.Enemies.TankCannon;

namespace Kriptok.Games.Alien.Entities.Enemies
{
    internal class TankCannon : ProcessBase<TankCannonView>
    {
        private const float pi4 = (float)(720d / (Math.PI * 2d));
        private const float angleModifier = (float)(((Math.PI * 2) / 16) / 2);

        /// <summary>
        /// Mapa de durezas.
        /// </summary>
        private readonly FastBitmap8 hadnesses;

        /// <summary>
        /// Dirección de movimiento.
        /// </summary>
        private readonly int direction;

        /// <summary>
        /// Contador de la frecuencia de disparo.
        /// </summary>
        private int disparo = 0;

        private IQuery<bool?> outOfScreen;

        /// <summary>
        /// Energía del tanque.
        /// </summary>
        private int energia = 2500;

        public TankCannon(FastBitmap8 hardnesses, int x, int y, int direction)
            : base(new TankCannonView())
        {
            this.hadnesses = hardnesses;
            this.direction = direction;
            Location.X = x;
            Location.Y = y;
            Location.Z = 10f;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            disparo = Global.TankShootFrequency;
            outOfScreen = h.GetOutOfScreenQuery();

            // Crea el cuerpo del tanque.
            Add(new TankBody(this));
        }

        protected override void OnBegin()
        {
            // Espera a que este dentro de pantalla
            While(() => outOfScreen.Result.GetValueOrDefault(true), () => Frame());

            // Repite mientras haya energia
            While(() => energia > 0, () =>
            {
                // Comprueba si ha tocado algun obstaculo del escenario		
                if (hadnesses.Sample((ushort)((Location.X + 10f) * 0.5f).Clamp(0f, hadnesses.Width - 1f), (ushort)(Location.Y * 0.5f)) != 24)
                {
                    Location.X += direction;
                }

                // Apunto al jugador.                
                View.Graph = ((MathHelper.SimplifyAngle(GetAngle2D(Global.MainRobot) + angleModifier) * pi4).Floor() / 45);

                if (!outOfScreen.Result.GetValueOrDefault(true))
                {
                    if (disparo == 0)
                    {
                        var seg = TankCannonView.GetVertices(GetLocation2D(), View.Graph);
                        // Add(new TankShot(Location.XY(), MathHelper.DegreesToRadians(View.Graph * 22.5f)));                    
                        Add(new TankShot(seg.V1, seg.GetDirectionVector().GetAngle()));

                        disparo = Global.TankShootFrequency;
                    }
                    else
                    {
                        disparo--;
                    }
                }

                // // Si se estaba moviendo y sale de la pantalla, muere.
                // if (outOfScreen.Result)
                // {
                //     Die();
                //     return;
                // }

                Frame();
            });

            // El canion del tanque ha sido destruido            
            Add(new Explosion1(Location));
            Global.puntuacion += 100;
        }

        /// <summary>
        /// Indica si el tanque ya no está en pantalla.
        /// </summary>        
        internal bool IsOutOfScreen() => !IsAlive() || outOfScreen.Result.GetValueOrDefault(true);

        /// <summary>
        /// Disparo del tanque.
        /// </summary>
        public class TankShot : EntityBase<SpriteView>
        {
            private IQuery<bool?> outScreen;
            private ISingleCollisionQuery<Robot> robotColl;
            private ISingleCollisionQuery<RobotLegs> robotLegsColl;

            public TankShot(Vector2F location, float angle) : base(new SpriteView(typeof(TankBody).Assembly, "Assets.Images.TankShot.png"))
            {
                Location.X = location.X;
                Location.Y = location.Y;

                Location.Z = -15f;
                Angle.Z = angle;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                h.CollisionType = Collision2DTypeEnum.Auto;

                h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.ESCO_AT1.WAV").Play();

                outScreen = h.GetOutOfScreenQuery();
                robotColl = h.GetCollision2D<Robot>();
                robotLegsColl = h.GetCollision2D<RobotLegs>();
            }

            protected override void OnFrame()
            {
                if (outScreen.Result.GetValueOrDefault(false))
                {
                    Die();
                    return;
                }
                else if (robotColl.OnCollision() || robotLegsColl.OnCollision())
                {
                    Add(new Impact(Location));
                    Global.energia_robot -= 1000;
                    Die();
                    return;
                }
                else
                {
                    Advance2D(5f);
                }
            }
        }

        /// <summary>
        /// Cuerpo del tanque.
        /// </summary>
        public class TankBody : ProcessBase<SpriteView>
        {
            private readonly TankCannon cannon;
            private ISingleCollisionQuery<RobotShot> shotColl;
            private ISingleCollisionQuery<Explosion3> exp3Coll;
            private IQuery<bool?> outOfScreen;
            private int energy = 2800;

            public TankBody(TankCannon cannon) : base(new SpriteView(typeof(TankBody).Assembly, "Assets.Images.Tank0.png"))
            {
                this.cannon = cannon;
                Location.Z = 15;
            }

            protected override void OnStart(ProcessStartHandler h)
            {
                base.OnStart(h);

                h.CollisionType = Collision2DTypeEnum.Auto;

                shotColl = h.GetCollision2D<RobotShot>();
                exp3Coll = h.GetCollision2D<Explosion3>();
                outOfScreen = h.GetOutOfScreenQuery();
            }

            protected override void OnBegin()
            {
                While(() => energy > 0, () =>
                {
                    // Comprueba si le ha tocado el disparo del robot                    
                    if (shotColl.OnCollision(out RobotShot playerShot))
                    {
                        Add(new Impact(playerShot.Location));

                        playerShot.Die();

                        if (HasFather())
                        {
                            // Quien tiene la energia es el proceso que llamo a este
                            cannon.energia = cannon.energia - 175;
                        }
                        else
                        {
                            energy = energy - 175;
                        }
                    }
                    else if (exp3Coll.OnCollision())
                    {
                        if (HasFather())
                        {
                            cannon.energia = cannon.energia - 100;
                        }
                        else
                        {
                            energy = energy - 100;
                        }
                    }

                    if (HasFather())
                    {
                        // Coge la posicion de la torre del tanque
                        Location.X = cannon.Location.X;
                        Location.Y = cannon.Location.Y;
                    }
                    else if (!outOfScreen.Result.GetValueOrDefault(true))
                    {
                        // Si no está en pantalla no gasto recursos generando objetos.
                        Add(new Smoke2(Location.X + Rand.Next(-4, 4), Location.Y + Rand.Next(-4, 4)));
                    }

                    // // Si se estaba moviendo y sale de la pantalla, muere.
                    // if (outOfScreen.Result)
                    // {
                    //     Die();
                    //     return;
                    // }

                    Frame();
                });

                Add(new Explosion2(Location, 1f));

                Global.puntuacion += 50;
            }

            public override Vector3F GetRenderLocation()
            {
                if (HasFather())
                {
                    return new Vector3F(cannon.GetRenderLocation().XY(), Location.Z);
                }

                return base.GetRenderLocation();
            }

            private bool HasFather() => cannon != null && cannon.IsAlive();
        }

        internal class TankCannonView : IndexedSpriteView
        {
            private static readonly Vector2F[][] points = new Vector2F[][]
            {
                new Vector2F [2] { new Vector2F(12f, -1f), new Vector2F(41f, -1f) },
                new Vector2F [2] { new Vector2F(10f, 3f), new Vector2F(37f, 13f) },
                new Vector2F [2] { new Vector2F(7f, 6f), new Vector2F(27f, 26f) },
                new Vector2F [2] { new Vector2F(3f, 9f), new Vector2F(14f, 32f) },

                new Vector2F [2] { new Vector2F(-2f, 9f), new Vector2F(-2f, 34f) },
                new Vector2F [2] { new Vector2F(-7f, 7f), new Vector2F(-19f, 31f) },
                new Vector2F [2] { new Vector2F(-9f, 2f), new Vector2F(-31f, 22f) },
                new Vector2F [2] { new Vector2F(-11f, -2f), new Vector2F(-39f, 10f) },

                new Vector2F [2] { new Vector2F(-11f, -5f), new Vector2F(-41f, -5f) },
                new Vector2F [2] { new Vector2F(-9f, -10f), new Vector2F(-37f, -21f) },
                new Vector2F [2] { new Vector2F(-6f, -11f), new Vector2F(-27f, -32f) },
                new Vector2F [2] { new Vector2F(0f, -12f), new Vector2F(-13f, -40f) },

                new Vector2F [2] { new Vector2F(3f, -13f), new Vector2F(3f, -42f) },
                new Vector2F [2] { new Vector2F(6f, -11f), new Vector2F(19f, -38f) },
                new Vector2F [2] { new Vector2F(9f, -8f), new Vector2F(30f, -29f) },
                new Vector2F [2] { new Vector2F(9f, -4f), new Vector2F(39f, -16f) },
            };

            public TankCannonView() : base(typeof(TankCannon).Assembly, "Assets.Images.TankCannon.png", 4, 4)
            {
            }

#if DEBUG
            public override void Render(IRenderContext context, Vector2F location, float rotation)
            {
                base.Render(context, location, rotation);
                
                // var seg = GetVertices(location, Graph);
                // 
                // context.Graphics.DrawLine(Pens.Red,
                //     context.Transform3D(new Vector3F(seg.V0, 0f)).XY().ToPointF(),
                //     context.Transform3D(new Vector3F(seg.V1, 0f)).XY().ToPointF());
            }
#endif
            internal static Segment2D GetVertices(Vector2F location, int index)
            {
                // Si esto alguna vez se generaliza, habría que agregar las transformaciones por 
                // rotación, escalado, etcétera.
                var arr = points[index];
                var pt1 = arr[0].Plus(location);
                var pt2 = arr[1].Plus(location);
                return new Segment2D(pt1, pt2);
            }
        }
    }
}
