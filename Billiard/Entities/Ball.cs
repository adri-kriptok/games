using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Views.Sprites;
using System;
using Kriptok.Audio;

namespace Billiard.Entities
{
    /// <summary>
    /// Proceso bola
    /// Maneja la bola
    /// </summary>
    public class Ball : ProcessBase<SpriteView>
    {
        private const float angleModifier = (float)(Math.PI / 96);

        /// <summary>
        /// Direccion de movimiento.
        /// </summary>
        public float MovementAngle = 0;

        /// <summary>
        /// Velocidad actual de bola.
        /// </summary>
        public float Speed = 0;

        /// <summary>
        /// Velocidad acumulada.
        /// </summary>
        private float acumulatedSpeed = 0;

        /// <summary>
        /// Variables auxiliares para cálculo de movimientos.
        /// </summary>
        private float x_resol, y_resol;

        /// <summary>
        /// Ultima posicion.
        /// </summary>
        private float ultima_pos_x, ultima_pos_y;

        /// <summary>
        /// Variables utilizadas para calcular el incremento por el movimiento.
        /// </summary>
        private float incr_x, incr_y;

        /// <summary>
        /// Velocidad final despues de la colision.
        /// </summary>
        private float veloc_final_x, veloc_final_y;

        /// <summary>
        /// Sonido de la bola golpeando contra los límites de la mesa.
        /// </summary>
        private ISoundHandler bandSound;

        /// <summary>
        /// Sonido de bolas golpeando unas con otras.
        /// </summary>
        private ISoundHandler billarSound;

        public Ball(int x, int y, string resource) : base(new SpriteView(typeof(Ball).Assembly, resource))
        {
            Location.X = x;
            Location.Y = y;

            Radius = 15;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            this.bandSound = h.Audio.GetWaveHandler(Global.BandaSound);
            this.billarSound = h.Audio.GetSoundHandler("BILLAR0.WAV");
        }

        protected override void OnBegin()
        {
            // Pone la sombra de la bola
            Add(new BallShadow(this));      

            x_resol = Location.X;
            y_resol = Location.Y;

            Loop(() =>
            {
                acumulatedSpeed += Speed;

                // Hasta que pare
                While(() => acumulatedSpeed > 100f, () =>
                {
                    acumulatedSpeed -= 100f;        // A cada paso decrementa la velocidad

                    ultima_pos_x = x_resol;   // Guarda posicion
                    ultima_pos_y = y_resol;

                    // Mueve la bola
                    incr_x = PolarVector.ProjectX(MovementAngle, 1f);
                    incr_y = PolarVector.ProjectY(MovementAngle, 1f);
                    x_resol += incr_x;
                    y_resol += incr_y;

                    // Comprueba el rebote con las bandas de manera vertical
                    if ((y_resol <= 104f && incr_y < 0f) ||
                        (y_resol >= 374f && incr_y > 0f))
                    {
                        Speed = Speed * 7f / 10f;               // Reduce la velocidad

                        // Hace un sonido de rebote.                        
                        bandSound.Play();

                        // Cambia el angulo para que rebote
                        MovementAngle = MathHelper.GetAngleF(
                            Location.X,
                            Location.Y,
                            Location.X + incr_x,
                            Location.Y - incr_y) + Global.EffectModifier.X * angleModifier;

                        // Quita cualquier efecto que existiera
                        Global.EffectModifier.Y = 80f;
                        Global.EffectModifier.X = 0f;
                    }

                    // Comprueba el rebote con las bandas de manera horizontal
                    if ((x_resol <= 62f && incr_x < 0f) ||
                        (x_resol >= 579f && incr_x > 0f))
                    {
                        Speed = Speed * 7f / 10f;

                        // Hace un sonido de rebote.                        
                        bandSound.Play();

                        MovementAngle = MathHelper.GetAngleF(
                            Location.X,
                            Location.Y,
                            Location.X - incr_x,
                            Location.Y + incr_y) + Global.EffectModifier.X * angleModifier;

                        Global.EffectModifier.Y = 80f;
                        Global.EffectModifier.X = 0f;
                    }

                    // Carga las coordenadas reales del grafico con las temporales
                    Location.X = x_resol;
                    Location.Y = y_resol;

                    // Comprueba si ha chocado con una bola                    
                    foreach (var ball in Radius2DCollisions<Ball>())
                    {
                        // Mira si esta colision no esta mirada y no es con esta bola
                        if (!ball.Equals(Global.LastBall) || !Equals(Global.CollisionBall))
                        {
                            // Guarda los identificadores para proximas comprobaciones
                            Global.CollisionBall = this;
                            Global.LastBall = ball;

                            // Comprueba si ha chocado la bola blanca con la amarilla
                            if ((Global.CollisionBall.Equals(Global.WhiteBall) && Global.LastBall.Equals(Global.YellowBall)) ||
                                (Global.CollisionBall.Equals(Global.YellowBall) && Global.LastBall.Equals(Global.WhiteBall)))
                            {
                                Global.CollisionOtherBall = true;
                            }

                            // Mira que bola tiene el turno
                            if (Global.CurrentTurn == 1)
                            {
                                // Y comprueba si choca con la roja
                                if ((Global.CollisionBall.Equals(Global.WhiteBall) && Global.LastBall.Equals(Global.RedBall)) ||
                                    (Global.CollisionBall.Equals(Global.RedBall) && Global.LastBall.Equals(Global.WhiteBall)))
                                {
                                    Global.CollisionRedBall = true;
                                }
                            }
                            else
                            {
                                if ((Global.CollisionBall.Equals(Global.RedBall) && Global.LastBall.Equals(Global.YellowBall)) ||
                                    (Global.CollisionBall.Equals(Global.YellowBall) && Global.LastBall.Equals(Global.RedBall)))
                                {
                                    Global.CollisionRedBall = true;
                                }
                            }

                            billarSound.Play();

                            acumulatedSpeed += 100;

                            Collision(ball);
                        }
                        else
                        {
                            // Si el identificador es el de la propia bola
                            // borra cualquier resenia del choque
                            if (Global.CollisionBall != null && Global.CollisionBall.Equals(this))
                            {
                                Global.LastBall = null;
                            }
                        }

                        x_resol = ultima_pos_x;     // Restaura posicion
                        y_resol = ultima_pos_y;
                    }
                });

                Location.X = x_resol.Clamp(62f, 579f);
                Location.Y = y_resol.Clamp(104f, 374f);

                Frame();

                // Parando las bolas
                if (Speed > 10f)
                {
                    Speed -= 10f;
                    if (Global.EffectModifier.Y > 80f)
                    {
                        Global.EffectModifier.Y -= 2f;
                        Speed += (Global.EffectModifier.Y - 80f) * 0.1f;
                    }
                    if (Global.EffectModifier.Y < 80f)
                    {
                        Global.EffectModifier.Y += 1f;
                        Speed -= (80f - Global.EffectModifier.Y) * 0.5f;
                    }
                }
                else
                {
                    // Se ha parado completamente
                    Speed = 0f;
                }
            });
        }

        private void Collision(Ball ball)
        {
            // Halla el angulo que hay entre las dos bolas que chocan
            var finalAngle = GetAngle2D(ball) + Global.EffectModifier.X * angleModifier;

            // Las bolas rebotan
            // Primero coge los incrementos horizontal y vertical de una bola
            incr_x = PolarVector.ProjectX(MovementAngle, Speed);
            incr_y = PolarVector.ProjectY(MovementAngle, Speed);

            // Y los mismo incrementos de la otra
            ball.incr_x = PolarVector.ProjectX(ball.MovementAngle, ball.Speed);
            ball.incr_y = PolarVector.ProjectY(ball.MovementAngle, ball.Speed);

            // Suma la velocidades, que es lo mismo que la fuerza total
            Global.WhiteBallTotalSpeed = Speed + ball.Speed;

            // Suma los incrementos sacando dos que es igual a la velocidades
            veloc_final_x = incr_x + ball.incr_x;
            veloc_final_y = incr_y + ball.incr_y;

            // Halla la longitud del vector entre las dos bolas,
            // que es proporcional a la suma de las velocidades
            var length = MathHelper.GetDistanceF(
                Location.X,
                Location.Y,
                Location.X + veloc_final_x,
                Location.Y + veloc_final_y) * Global.EffectModifier.Y * 0.01f;

            // Borra los efectos una vez que han colisionado
            Global.EffectModifier.Y = 80f;
            Global.EffectModifier.X = 0f;

            // A la bola que choca se le resta el vector
            // total del choque
            incr_x -= PolarVector.ProjectX(finalAngle, length);
            incr_y -= PolarVector.ProjectY(finalAngle, length);

            // Y se hallan los nuevos valores para dicha bola
            MovementAngle = MathHelper.GetAngleF(
                Location.X,
                Location.Y,
                Location.X + incr_x,
                Location.Y + incr_y);

            Speed = MathHelper.GetDistanceF(
                Location.X,
                Location.Y,
                Location.X + incr_x,
                Location.Y + incr_y);

            // A la bola que recibe el choque se le suma
            // el vector total que ha salido del choque
            ball.incr_x += PolarVector.ProjectX(finalAngle, length);
            ball.incr_y += PolarVector.ProjectY(finalAngle, length);

            // Y se actualizan sus valores de angulo y velocidad
            ball.MovementAngle = MathHelper.GetAngleF(
                Location.X,
                Location.Y,
                Location.X + ball.incr_x,
                Location.Y + ball.incr_y);

            ball.Speed = MathHelper.GetDistanceF(
                Location.X,
                Location.Y,
                Location.X + ball.incr_x,
                Location.Y + ball.incr_y);

            // Por ultimo se hace una media con la fuerza inicial
            // para que en vez de ser proporcional, sea identica
            Global.YellowBallTotalSpeed = Speed + ball.Speed;
            Speed = Global.WhiteBallTotalSpeed * Speed / Global.YellowBallTotalSpeed;
            ball.Speed = Global.WhiteBallTotalSpeed * ball.Speed / Global.YellowBallTotalSpeed;

            acumulatedSpeed = Speed;
            ball.acumulatedSpeed = ball.Speed;

            // Aplico una repulsión para que no entren en bucle de colisiones.
            var repelAngle = GetAngle2D(ball);
            var neg = PolarVector.ProjectX(repelAngle, -0.5f);
            var pos = PolarVector.ProjectX(repelAngle, 0.5f);
            
            x_resol = x_resol + neg;
            y_resol = y_resol + neg;
            ball.x_resol = ball.x_resol + pos;
            ball.y_resol = ball.y_resol + pos;

            Location.X = x_resol;
            Location.Y = y_resol;
            ball.Location.X = ball.x_resol;
            ball.Location.Y = ball.y_resol;
        }
    }
}
