using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Drawing.Shapes;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Objects.Base;
using Kriptok.Objects.Collisions;
using Kriptok.Objects.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    class Ball : ProcessBase<IndexedSpriteView>
    {
        private const float initialAngle = -(float)(3 * Math.PI / 8);
        private const float pi32 = (float)(Math.PI / 32);
        private const float pi60 = (float)(Math.PI / 60);
        private const float pi78 = (float)(7 * Math.PI / 8);
        private const float pi98 = (float)(9 * Math.PI / 8);

        private const float pi38 = (float)(3 * Math.PI / 8);
        private const float pi58 = (float)(5 * Math.PI / 8);

        private const float pi8 = (float)(Math.PI / 8);
        private const float pi158 = (float)(15 * Math.PI / 8);
        // private const float mpi78 = -pi78;
        // private const float mpi98 = -pi98;
        // private const float mpi8 = -pi8;
        private const float mpi38 = -pi38;
        private const float mpi58 = -pi58;

        private static readonly float randomAngle = MathHelper.DegreesToRadians(4f);

        private const float movementDelta = 1f / 16f;

        private readonly Racket racket;

        /// <summary>
        /// Indica si está en modo demo.
        /// </summary>
        private bool demo = false;

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
        private float currentSpeed = 400f / movementDelta;

        /// <summary>
        /// Indica si en el próximo frame debe agregar dos bolas más.
        /// </summary>
        internal bool CreateBalls = false;

        private Ball(Racket racket) : base(new IndexedSpriteView(typeof(Ball).Assembly, "Assets.Images.Ball.png", 2, 1))
        {
            this.racket = racket;
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
            return sticked ? GetStickedLocation() : base.GetRenderLocation();
        }

        private Vector3F GetStickedLocation()
        {
            var rl = racket.GetRenderLocation();
            return new Vector3F(rl.X, rl.Y - 8f, Location.Z);
        }

        protected override void OnBegin()
        {
            Location.Z = 3;

            // Inicia variables propias del proceso
            var speedIncrement = 4f / movementDelta;            
            var avelocidad = 0f;

            Loop(() =>
            {
                // // Si no esta en modo pegamento bola nueva
                // if (!sticked)
                // {
                //     nbolas++;
                //     parado = 1;
                //     pos_bol_raq = 0;
                // }
                // else
                // {
                //     // Pegamento
                //     pos_bol_raq = Location.X - racket.Location.X;
                // }
                While(() => sticked, () =>
                {
                    // Repite mientras este en modo pegamento
                    // Coge las coordenadas respecto a la raqueta
                    Location = GetStickedLocation();

                    // Si se pulsa la tecla espacio
#if !DEBUG && !SHOWFPS
                    if (Input.Button03() || demo)  // Lanza la bola
                    {
#endif
                        angulo0 = Rand.Next(0, 1) == 0 ? angulo0 : MathHelper.PIF - angulo0;
                        sticked = false;                    
#if !DEBUG && !SHOWFPS
                    }
#endif
                    Frame();
                });

                Repeat(() =>
                {
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
                    avelocidad = currentSpeed;

                    // Comprueba la trayectoria y la colision con los ladrillos
                    while (avelocidad > 100)
                    {
                        var previewsLocation = Location.XY();

                        var incX = PolarVector.ProjectX(angulo0, movementDelta);
                        var incY = PolarVector.ProjectY(angulo0, movementDelta);

                        var newXY = previewsLocation.Plus(incX, incY);

                        avelocidad -= 100;


                        //         ultima_x_resol=x_resol;
                        //         ultima_y_resol=y_resol;
                        // 
                        //         incr_x=get_distx(angulo0,100);
                        //         x_resol+=incr_x;
                        //         incr_y=get_disty(angulo0,100);
                        //         y_resol+=incr_y;
                        //                                                    

                        // Toca la raqueta
                        var dist_raqueta = Location.X - racket.Location.X;

                        // Toca la bola al ojo de la raqueta
                        if (newXY.Y >= 180f && newXY.Y <= 188f && incY > 0f &&
                            (dist_raqueta > -18f - racket.currentSize &&
                             dist_raqueta < 18f + racket.currentSize))
                        {
                            angulo0 = MathHelper.SimplifyAngle(MathHelper.GetAngleF(incX, -incY) + (dist_raqueta * pi60) * 0.5f);

                            //// Angulo demasiado horizontales no estan permitidos
                            angulo0 = angulo0.Clamp(pi98, pi158);

                            // if (angulo0 > pi118 && angulo0 < pi138)
                            // {
                            //     if (incX > 0)
                            //     {
                            //         angulo0 = pi138;
                            //     }
                            //     else
                            //     {
                            //         angulo0 = pi118;
                            //     }
                            // }


                            // var newIncX = PolarVector.ProjectX(angulo0, movementDelta);
                            // // var newIncY = PolarVector.ProjectY(angulo0, movementDelta);
                            // 
                            // if (Math.Sign(newIncX) != Math.Sign(incX))
                            // {
                            //     incX = -incX;
                            // 
                            //     angulo0 = MathHelper.SimplifyAngle(MathHelper.GetAngleF(incX, -incY) + (dist_raqueta * pi60) * 0.5f);
                            // 
                            //     // Angulo demasiado horizontales no estan permitidos
                            //     angulo0 = angulo0.Clamp(pi98, pi158);
                            // }

                            // Raqueta pegamento
                            if (racket.Sticky) 
                            {                                
                                sticked = true;
                                break;                                
                            }

                            // sound(s_raqueta,100,256);
                        }

                        // Colisiona con el lado horizontal del tablero
                        if (newXY.Y <= 12f && incY < 0f)
                        {
                            //angulo0=fget_angle(0,0,incr_x,-incr_y);
                            angulo0 = MathHelper.GetAngleF(incX, -incY);
                            //sound(s_raqueta, 80, 500);
                        }

                        // Colisiona con los lados verticales de la pantalla
                        if ((newXY.X <= 12 && incX < 0f) || (newXY.X >= 260 && incX > 0f))
                        {
                            angulo0 = MathHelper.GetAngleF(-incX, incY);
                            //angulo0 =fget_angle(0,0,-incr_x,incr_y);
                            //sound(s_raqueta,80,600);
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
                                        // angulo0 =fget_angle(0,0,-incr_x,incr_y)+rand(-4000,4000);
                                        angulo0 = MathHelper.SimplifyAngle(MathHelper.GetAngleF(-incX, incY) + Rand.NextF(-randomAngle, randomAngle));

                                        // // Empuja la bola desde el ladrillo
                                        // newXY.X = previewsLocation.X;

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

                                        // sound(s_metal,100,256);
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

                                        // sound(s_metal, 100, 256);
                                    }

                                    // Indico que colisionó, y se lo informo al ladrillo.
                                    hitFlag = true;
                                    brick.Hit();
                                }
                                else if(brick.CanBeDestroyed())
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



                        //        if (id_ladrillo==collision(TYPE ladrillo))
                        //            }
                        //    
                        //         } else {
                        //    
                        //        }
                        //    
                        //    
                        //        if (id_ladrillo.graph>=20 && id_ladrillo.graph<30)
                        //            id_ladrillo.estado=1;   // Ladrillos que no caen (indestructibles)
                        //         } else {
                        //            // Ladrillos a los que tienes que dar varios toques
                        //            if (id_ladrillo.graph>=30 && id_ladrillo.graph<33)
                        //                id_ladrillo.graph++;    // Cambia el grafico
                        //                if (id_ladrillo.graph==33)  // Si ha llegado a roto, elimina el ladrillo
                        //                    signal(id_ladrillo,s_kill);
                        //                    nladrillos--;
                        //                }
                        //             } else {// Aqui se comprueban ladrillos normales
                        //                // Aleatoriamente va creando pildoras
                        //                if (rand(0,100)<20)
                        //                    pildora(id_ladrillo.x,id_ladrillo.y,rand(202,209));
                        //                }
                        //                // Crea un ladrillo de los que caen
                        //                caido(id_ladrillo.x,id_ladrillo.y,id_ladrillo.graph);
                        //                // Elimina el ladrillo
                        //                signal(id_ladrillo,s_kill);
                        //                nladrillos--;
                        //    
                        //            }
                        //        }
                        //    
                        //    }                            
                        //}
                    }

                    Frame();
                    // Repite hasta que se salga de pantalla o en modo pegamento
                }, () => Location.Y > 200f || sticked);

                // -------------------------------------
                // Bola perdida
                // -------------------------------------
                if (!sticked)
                {
                    // Reinicia esta bola
                    // velocidad = 400;
                    // angulo0 = 3 * pi / 8;
                                        
                    var ballCount = Find.All<Ball>().Count() - 1;       
                    if (ballCount > 0)   // Si se tienen mas bolas se sale                       
                    {
                        Die();
                        return;
                    }
                    else
                    {
                        // base.FadeOff();
                        // base.FadeOn();
                        // // Quita una raqueta cuando es la ultima bola
                        // fade_off();
                        // fade_on();
                        // camb_dir=1;
                        // rebota=1;
                        // graph=1;
                        // id_raqueta.graph=3;
                        // if(vidas>0)
                        // { 
                        //     signal(pequenio_r[vidas-1],s_kill);
                        // }
                        // signal(type pildora,s_kill);
                        // if (NOT demo)   // Si no esta en modo demo quita una vida
                        // { 
                        //     vidas--;
                        //     if (vidas<0)    // Si no quedan vidas se acaba el juego
                        //     { 
                        //         fin_juego=1;
                        //         BREAK;
                        //     }
                        // }
                    }
                }
                Frame();
            });
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
