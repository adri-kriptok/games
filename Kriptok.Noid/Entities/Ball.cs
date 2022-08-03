using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Drawing.Shapes;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Noid.Scenes;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    class Ball : EntityBase<IndexedSpriteView>
    {
        /// <summary>
        /// Ángulo inicial.
        /// </summary>
        private const float initialAngle = -(float)(3 * Math.PI / 8);

        /// <summary>
        /// Incremento de velocidad de la bola.
        /// </summary>
        private const float speedIncrement = 4f / movementDelta;


        private const float modifier = 0.5f;
        private const float speedModifier = 100f;

        /// <summary>
        /// Cuanto avanza la bola por cada "sub-frame".
        /// </summary>
        private const float movementDelta = 1f / 16f * modifier;

        private const float pi32 = (float)(Math.PI / 32);
        private const float pi60 = (float)(Math.PI / 60);
        private const float pi78 = (float)(7 * Math.PI / 8);
        private const float pi98 = (float)(9 * Math.PI / 8);

        private const float pi38 = (float)(3 * Math.PI / 8);
        private const float pi58 = (float)(5 * Math.PI / 8);

        private const float pi8 = (float)(Math.PI / 8);
        private const float pi158 = (float)(15 * Math.PI / 8);        
        private const float mpi38 = -pi38;
        private const float mpi58 = -pi58;

        private static readonly float randomAngle = MathHelper.DegreesToRadians(4f);

        /// <summary>
        /// Raqueta/jugador.
        /// </summary>
        private readonly Racket racket;        

        /// <summary>
        /// Indica si está en modo demo.
        /// </summary>
        private readonly bool demo = false;

        /// <summary>
        /// Indica si está agarrada a la paleta.
        /// </summary>
        private bool sticked = false;

        /// <summary>
        /// Ángulo de movimiento.
        /// </summary>
        private float angulo0 = initialAngle;

        /// <summary>
        /// Indica si la bola debe rebotar.
        /// </summary>
        private bool bounces = true;

        /// <summary>
        /// Velocidad a la que se desplaza la bola. Cuando se crean nuevas bolas, éstas
        /// tienen que ir a la misma velocidad que las que las crea.
        /// </summary>
        private float currentSpeed = 250f / movementDelta;

        /// <summary>
        /// Indica si en el próximo frame debe agregar dos bolas más.
        /// </summary>
        internal bool CreateBalls = false;

        /// <summary>
        /// Velocidad acumulada de la bola.
        /// </summary>
        private float acumulatedSpeed = 0f;

        private Ball(Racket racket) : base(new IndexedSpriteView(typeof(Ball).Assembly, "Assets.Images.Ball.png", 2, 1))
        {
            this.racket = racket;
            Location.X = 140;
        }

        public Ball(Racket racket, bool sticked, bool demo) : this(racket)
        {
            this.demo = demo;
            this.sticked = sticked;
            
            // Grafico de la bola.
            View.Graph = 0;
        }

        /// <summary>
        /// Crea una bola en base a otra.
        /// </summary>        
        private Ball(Ball ball, float angleModifier) : this(ball.racket)
        {
            Location = ball.Location;
            angulo0 = ball.angulo0 + angleModifier;
            currentSpeed = ball.currentSpeed;
            bounces = ball.bounces;
            View.Graph = ball.View.Graph;
        }

        public override Vector3F GetRenderLocation()
        {
            // Mientras esté pegada a la raqueta, se mueve con ella, pero para asegurar
            // que no haya ningún "delay" visual, al momento de renderizar, que tome directamente
            // la misma ubicación que la de la raqueta.
            return sticked ? GetStickedLocation() : base.GetRenderLocation();
        }

        private Vector3F GetStickedLocation()
        {
            var rl = racket.GetRenderLocation();
            return new Vector3F(rl.X, rl.Y - 8f, Location.Z);
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            Location.Z = 3;

        }

        protected override void OnFrame()
        {         
            if(sticked)
            {
                // Repite mientras este en modo pegamento
                // Coge las coordenadas respecto a la raqueta
                Location = GetStickedLocation();

                // Si se pulsa la tecla espacio.
                if (demo || Input.Button03())  // Lanza la bola
                {
                    angulo0 = Rand.Next(0, 1) == 0 ? angulo0 : MathHelper.PIF - angulo0;

                    //Audio.PlaySound(Sounds.ReleaseBallSound);

                    sticked = false;

                }

                return;
            }

            // Obtengo los ladrillos que están actualmente vivos para validar colisiones.
            var livingBricks = Find.All<Brick>().Where(p => p.Bounces).ToArray();

            // Multibola
            if (CreateBalls)       
            {
                Add(new Ball(this, -pi32));
                Add(new Ball(this, +pi32));                        
                CreateBalls = false;
            };

            // En cada impresion de pantalla suma avelocidad con velocidad
            acumulatedSpeed = currentSpeed;

            // Comprueba la trayectoria y la colision con los ladrillos
            while (acumulatedSpeed > speedModifier)
            {
                var previewsLocation = Location.XY();

                var incX = PolarVector.ProjectX(angulo0, movementDelta);
                var incY = PolarVector.ProjectY(angulo0, movementDelta);

                var newXY = previewsLocation.Plus(incX, incY);

                acumulatedSpeed -= speedModifier;

                // Toca la raqueta
                var dist_raqueta = Location.X - racket.Location.X;

                // Toca la bola al ojo de la raqueta
                if (newXY.Y >= 180f && newXY.Y <= 188f && incY > 0f &&
                    (dist_raqueta > -18f - racket.currentSize &&
                        dist_raqueta < 18f + racket.currentSize))
                {
                    angulo0 = MathHelper.SimplifyAngle(MathHelper.GetAngleF(incX, -incY) + (dist_raqueta * pi60) * 0.5f);

                    // Angulo demasiado horizontales no estan permitidos.
                    angulo0 = angulo0.Clamp(pi98, pi158);

                    // Raqueta pegamento
                    if (racket.Sticky) 
                    {
                        angulo0 = initialAngle;
                        sticked = true;
                        break;                                
                    }
                            
                    Audio.PlayWave(Sounds.RacketSound);
                }

                // Colisiona con el lado horizontal del tablero
                if (newXY.Y <= 12f && incY < 0f)
                {
                    angulo0 = MathHelper.GetAngleF(incX, -incY);                            
                    Audio.PlayWave(Sounds.BoundSound);
                }

                // Colisiona con los lados verticales de la pantalla
                if ((newXY.X <= 12 && incX < 0f) || (newXY.X >= 260 && incX > 0f))
                {
                    angulo0 = MathHelper.GetAngleF(-incX, incY);
                    //angulo0 =fget_angle(0,0,-incr_x,incr_y);                            
                    Audio.PlayWave(Sounds.BoundSound);
                }

                // Actualiza las coordenadas reales
                Location.X = newXY.X;
                Location.Y = newXY.Y;

                // Comprueba si choca con un ladrillo                        
                if (OnCollision(livingBricks, out IEnumerable<Brick> bricks))
                {
                    // Flag para saber si colisionó o no contra un ladrillo.
                    var hitFlag = false;

                    // Utilizado para calcular intersecciones.
                    var p0 = Location.XY();

                    // Recorro los ladrillos encontrados.
                    foreach (var brick in bricks)
                    {
                        // Me fijo si sigue collisionando con este ladrillo.
                        if (brick.Bounces && (bounces || !brick.CanBeDestroyed()))
                        {
                            var p1 = brick.Location.XY();

                            // Lado de arriba.
                            var rY00 = p1.Plus(-8, -4);
                            var rY01 = p1.Plus(8, -4);

                            // Lado de abajo.
                            var rY10 = p1.Plus(-8, 4);
                            var rY11 = p1.Plus(8, 4);

                            var y0 = Vector2F.Intersection(p0, p1, rY00, rY01);
                            var y1 = Vector2F.Intersection(p0, p1, rY10, rY11);
#if DEBUG
                            // Lado izquierdo.
                            var rX00 = p1.Plus(-8, -4);
                            var rX01 = p1.Plus(-8, 4);

                            // Lado derecho.
                            var rX10 = p1.Plus(8, -4);
                            var rX11 = p1.Plus(8, 4);

                            var x0 = Vector2F.Intersection(p0, p1, rX00, rX01);
                            var x1 = Vector2F.Intersection(p0, p1, rX10, rX11);

                            if (!x0.HasValue && !y0.HasValue && !x1.HasValue && !y1.HasValue)
                            {
                                Debugger.Break();
                            }
#endif

                            if (((y0.HasValue && incY < 0f) ||
                                (y1.HasValue && incY > 0f)))
                            {
                                // Si pega de abajo, pero está bajando, lo ignoro.
                                // O de arriba cuando está subiendo.
                                continue;
                            }

                            // Vuelvo atrás el último movimiento, por las dudas.
                            Location.X = previewsLocation.X;
                            Location.Y = previewsLocation.Y;

                            // Si no encontré intersecciones con los lados horizontales.
                            // Colisiona con el lado vertical de un ladrillo
                            if (!y0.HasValue && !y1.HasValue)
                            {
                                angulo0 = MathHelper.SimplifyAngle(MathHelper.GetAngleF(-incX, incY) + Rand.NextF(-randomAngle, randomAngle));

                                if (angulo0 < pi8)
                                {
                                    angulo0 = pi8;
                                }
                                else if (angulo0 > pi158)
                                {
                                    angulo0 = pi158;
                                }
                                else if (angulo0 > pi78 && angulo0 < pi98)
                                {
                                    // Este es el caso más complicado, cuando el ángulo está
                                    // cerca de los 180 grados.

                                    // Me fijo para qué lado está yendo.
                                    if (angulo0 >= MathHelper.TwoPIF)
                                    {
                                        angulo0 = pi98;
                                    }
                                    else
                                    {
                                        angulo0 = pi78;
                                    }
                                }
                            }
                            else
                            {
                                // Colisiona con el lado horizontal del ladrillo                                
                                angulo0 = MathHelper.GetAngleF(incX, -incY) + Rand.NextF(-randomAngle, randomAngle);

                                if (angulo0 < mpi38 && angulo0 > mpi58)
                                {
                                    if (incX > 0)
                                    {
                                        angulo0 = mpi38;
                                    }
                                    else
                                    {
                                        angulo0 = mpi58;
                                    }
                                }

                                if (angulo0 > pi38 && angulo0 < pi58)
                                {
                                    if (incX > 0)
                                    {
                                        angulo0 = pi38;
                                    }
                                    else
                                    {
                                        angulo0 = pi58;
                                    }
                                }
                            }

                            // Indico que colisionó, y se lo informo al ladrillo.
                            hitFlag = true;
                            brick.Hit();
                        }
                        else if (brick.CanBeDestroyed())
                        {
                            // Si no rebota, es porque es una "super-ball"
                            brick.Hit();
                        }                                
                    }

                    if (hitFlag)
                    {
                        currentSpeed = Math.Min(16000, currentSpeed + speedIncrement);
                        break;
                    }
                }
            }

            // Repite hasta que se salga de pantalla.
            if (Location.Y > 200f)
            {
                Die();

                var ballCount = Find.All<Ball>().Count();

                // Si era la última bola.
                if (ballCount == 0)
                {
                    Scene.SendMessage(LevelSceneMessages.Lose);
                }

                return;
            }            
        }

        /// <summary>
        /// Libera la bola si estaba pegada a la raqueta.
        /// </summary>
        internal void Release() => sticked = false;

        private bool OnCollision(Brick[] livingBricks, out IEnumerable<Brick> bricks)
        {
            var circle = new Circle(Location.XY(), 3.75f);

            var list = new List<Brick>();

            foreach (var b in livingBricks)
            {
                if (Collision2DHelper.Collide(circle, b.GetRect()))
                {
                    list.Add(b);
                }
            }

            if (list.Count == 0)
            {
                bricks = null;
                return false;
            }
            else/* if (list.Count == 1)*/
            {
                bricks = list;
                return true;
            }
        }

        /// <summary>
        /// Indica que agarró una píldora que genera múltiples bolas.
        /// </summary>
        internal void MultiBallPicked() => CreateBalls = true;

        /// <summary>
        /// Convierte a la bola en una bola que no rebota.
        /// </summary>
        internal void SuperBallPicked()
        {
            bounces = false;
            View.Graph = 1;
        }

        /// <summary>
        /// Reduce la velocidad de la bola.
        /// </summary>
        internal void DecreaseSpeedPicked()
        {
            currentSpeed = Math.Max(currentSpeed - 400, 400);
        }
    }
}
