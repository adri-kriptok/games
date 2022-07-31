using Kriptok.Common;
using Kriptok.Extensions;
using Kriptok.Noid.Entities.Pills;
using Kriptok.Objects.Base;
using Kriptok.Objects.Collisions;
using Kriptok.Objects.Collisions.Queries;
using Kriptok.Views.Sprites;
using Kriptok.Views.Sprites.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Noid.Entities
{
    class Racket : ProcessBase<RacketView>
    {
        /// <summary>
        /// Máxima velocidad de movimiento.
        /// </summary>
        public const int MaxSpeed = 8;

        /// <summary>
        /// Indica si está en modo demo.
        /// </summary>
        private readonly bool demo;

        /// <summary>
        /// Bola por defecto a aplicar el "multi-ball".
        /// </summary>
        private Ball ball;

        /// <summary>
        /// Indica si la dirección en que debe moverse está invertida.
        /// </summary>
        private int backwardsControl = 1;

        /// <summary>
        /// Tamanio de la raqueta.
        /// </summary>
        internal int currentSize=4;

        /// <summary>
        /// Indica si la raqueta tiene pegamento.
        /// </summary>
        internal bool Sticky { get; private set; } = false;

        /// <summary>
        /// Indica si la raqueta puede disparar lasers.
        /// </summary>
        private bool laser = true;

        /// <summary>
        /// Indica si está listo para disparar un laser.
        /// </summary>
        private bool readyToFire = true;

        /// <summary>
        /// Control de colisiones con píldoras.
        /// </summary>
        private ISingleCollisionQuery<PillBase> pillCollision;

        public Racket(bool demo) : base(new RacketView())
        {
            this.demo = demo;

            // Coordenadas iniciales
            Location.X = 140;
            Location.Y = 188;
        }

        internal void SetBall(Ball ball)
        {
            this.ball = ball;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Rectangle;
            this.pillCollision = h.GetCollision2D<PillBase>();
        }

        protected override void OnBegin()
        {
            // Empieza con una raqueta normal
            View.Graph = 0;

            // Coordenadas iniciales
            Location.X = 140;
            Location.Y = 188;

            // Espera si no hay bola
            While(() => ball == null, () => Frame());

            // Incremento x.
            var incX = 0f;              

            Loop(() =>
            {
                // El jugador controla la raqueta
                if (!demo)
                {
                    // if (camb_dir == 1)
                    // {
                        // Comprueba la pulsacion de las teclas de los cursores
                        if (Input.Left() && incX > -MaxSpeed)
                        {
                            incX -= 4 * backwardsControl;  // Acelera
                        }
                        else if (Input.Right() && incX < MaxSpeed)
                        {
                            incX += 4 * backwardsControl;  // Acelera
                        }
                        else
                        {
                            // Ninguna tecla pulsada
                            if (incX > 0)
                            {
                                incX -= 2 * backwardsControl;
                            }
                            else if (incX < 0)
                            {
                                // Frena la raqueta
                                incX += 2 * backwardsControl;
                            }
                        }
                }
                else 
                {  
                    // El ordenador controla la raqueta (modo demo)
                //        if ((x>7) && (x<310)) x=id_bola.x; }
                //        if (scan_code!=0)     // El jugador toma el control de la raqueta
                //            delete_text(all_text);          // Borra textos
                //            signal(type texto_demo,s_kill); // Elimina el proceso que pone el texto explicativo
                //            demo=0;                         // Quita el modo demo
                //            nladrillos=0;                   // Reinicia variables
                //            fase=-1;
                //            vidas=3;
                //            puntuacion=0;
                //            write_int(1,310,180,2,&puntuacion); // Imprime puntuacion
                //        }
                //        if (puntuacion>200) // Si hace mas de 200 puntos cambia de fase (solo modo demo)
                //            nladrillos=0;
                //            fase=rand(-1,10);
                //            puntuacion=0;
                //        }
                }

                // Comprueba si se pulsa las teclas de disparo y se tiene la modalidad de disparo
                if ((Input.Button03() || (demo && Rand.Next(0, 15) == 0)) && laser)
                {
                    // Si no ha disparado esta vez
                    if (readyToFire)
                    {
                        // sound(s_fuego, 80, 400);  // Y realiza sonido

                        // Dispara los lasers...
                        Add(new Laser(Location.X - 16f, Location.Y - 8f));
                        Add(new Laser(Location.X + 16f, Location.Y - 8f));
                        if (!demo)
                        {
                            // Y lo deja preparado para disparar de uno en uno
                            readyToFire = false;
                        }
                    }
                }
                else
                {
                    // Permite volver a disparar
                    readyToFire = true;
                }

                //    // Comprueba cuando coges una p¡ldora de bonos                
                if (pillCollision.OnCollision(out PillBase pill))
                {
                    pill.Pick();                    
                //        if (id2.size==100)                  // Comprueba que no se halla cogido antes
                //            sound(s_pildora,80,256);
                //            puntuacion+=50;                 // Suma puntos
                //            switch (id2.graph);             // Comprueba que tipo de pildora es

                //                CASE 201:               // P¡ldora pegamento

                //                }
                //                CASE 202:               // P¡ldora de disparo
                //                    tamanio_raqueta=4;   // Pone el tamanio en normal
                //                    graph=6;            // Selecciona el grafico necesario
                //                    id3=get_id(TYPE bola);
                //                    While(() =>  (id3)
                //                        id3.parado=0;   // Quita el modo pegamento
                //                        id3=get_id(TYPE bola);
                //                    }
                //                }
                //                CASE 203:               // P¡ldora de vida extra
                //                    if (vidas<3)        // Si se tienen menos de 3 vidas, crea una mas
                //                        vidas++;
                //                        pequenio_r[vidas-1]=pequenia_raq(296,16+11*(vidas-1));
                //                    };
                //                }
                //                CASE 205:               // Reduce la raqueta
                //                 
                //                    if (tamanio_raqueta==0)  // Selecciona el grafico necesario
                //                        graph=2;
                //                     } else {
                //                        graph=3;
                //                    }
                //                    id3=get_id(TYPE bola);
                //                    While(() =>  (id3)
                //                        id3.parado=0;       // Quita el modo pegamento si existiera
                //                        id3=get_id(TYPE bola);
                //                    }
                //                }
                //                CASE 206:               // P¡ldora de velocidad
                //                    // Decrementa la velocidad de la bola si se puede
                //                    id3=get_id(TYPE bola);
                //                    While(() =>  (id3)
                //                        if (id3.velocidad>800)
                //                            id3.velocidad-=400;
                //                         } else {
                //                            id3.velocidad=400;
                //                        }
                //                        id3=get_id(TYPE bola);
                //                    }
                //                }             
                //                
                //                }
                //            }
                //        }
                //        id2.estado=1;   // Quita ese bonus
                }

                // Si no esta en demo mueve la paleta
                if (!demo)
                {
                    Location.X += incX;
#if DEBUG || SHOWFPS
                    Location.X = Find.All<Ball>().OrderByDescending(p => p.Location.Y).FirstOrDefault().IfNotNull(b => b.Location.X);
#endif
                }
                else
                {
                    // Tomo la X de la bola más cercana.
                    var bX = Find.All<Ball>().OrderByDescending(p => p.Location.Y).FirstOrDefault().IfNotNull(b => b.Location.X);
                    if (bX < Location.X)
                    {
                        Location.X -= Math.Min(4f, Location.X - bX);
                    } 
                    else if (bX > Location.X)
                    {
                        Location.X += Math.Min(4f, bX - Location.X);
                    }
                }

                // Limites de la pantalla segun el tamanio
                if (Location.X < 24f + currentSize)
                {
                    Location.X = 24f + currentSize;
                    incX = 0f;
                }
                if (Location.X > 249f - currentSize)
                {
                    Location.X = 249f - currentSize;
                    incX = 0f;
                }
                Frame();
            });
        }

        /// <summary>
        /// Evento a disparar cuando se agarra una píldora violeta que invierte las direcciones del teclado.
        /// </summary>
        internal void BackwardsPillPicked() => backwardsControl = backwardsControl * -1;

        /// <summary>
        /// Evento a disparar cuando agarró una píldora que genera múltiples bolas.
        /// </summary>
        internal void MultiBallPillPicked()
        {
            if (!ball.IsAlive())
            {
                ball = Find.All<Ball>().FirstOrDefault();
            }

            if (ball != null)
            {
                ball.MultiBallPicked();
            }
        }

        /// <summary>
        /// Evento a disparar cuando se come una píldora E.
        /// </summary>
        internal void ExtendRacketBallPillPicked()
        {
            // Comprueba que extension tiene actualmente
            if (currentSize < 8)
            {
                currentSize += 4;
                ReleaseBalls();
                laser = false;

                // Coloca el grafico dependiendo del tamanio
                UpdateGraph();
            }
        }

        /// <summary>
        /// Libera las bolas, si estaba en modo "sticked".
        /// </summary>
        private void ReleaseBalls()
        {
            if (Sticky)
            {
                Find.All<Ball>().ForEach(b => b.Release());
                Sticky = false;
            }
        }

        /// <summary>
        /// Evento a disparar cuando se come una píldora R.
        /// </summary>
        internal void ReduceRacketBallPillPicked()
        {
            // Comprueba que extension tiene actualmente
            if (currentSize > 0)
            {
                currentSize -= 4;
                ReleaseBalls();
                laser = false;

                // Coloca el grafico dependiendo del tamanio
                UpdateGraph();
            }
        }

        /// <summary>
        /// Evento a disparar cuando se come una píldora S.
        /// </summary>
        internal void StickyRacketPillPicked()
        {
            // Pone el tamanio en normal
            currentSize=4;   
            Sticky = true;
            laser = false;
            
            // Coloca el grafico dependiendo del tamanio
            UpdateGraph();            
        }

        /// <summary>
        /// Evento a disparar cuando se come una píldora S.
        /// </summary>
        internal void LaserPillPicked()
        {
            // Pone el tamanio en normal
            currentSize = 4;
            ReleaseBalls();
            laser = true;

            // Coloca el grafico dependiendo del tamanio
            UpdateGraph();
        }

        private void UpdateGraph()
        {
            if (Sticky)
            {
                View.Graph = 3;
            }
            else if (laser)
            {
                View.Graph = 4;
            }
            else
            {
                switch (currentSize)
                {
                    case 0: View.Graph = 1; break;
                    case 4: View.Graph = 0; break;
                    case 8: View.Graph = 2; break;
                }
            }
        }
    }

    class RacketView : IndexedSpriteView
    {
        public RacketView()
            : base(new Resource(typeof(RacketView).Assembly, "Assets.Images.PadNormal.png"), 1, 1)
        {
            Add(typeof(RacketView).Assembly, "Assets.Images.PadSmall.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadLarge.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadGlue.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadLaser.png", 1, 1);
        }
    }
}
