using Kriptok.Common;
using Kriptok.Extensions;
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

        private readonly bool demo;
        private Ball ball;

        /// <summary>
        /// Indica si la dirección en que debe moverse está invertida.
        /// </summary>
        private int backwardsControl = 1;

        /// <summary>
        /// Tamanio de la raqueta.
        /// </summary>
        internal int tamanio_raqueta=4;

        private float incX = 0;               // Incremento x

        private bool fuego_preparado = true;      // Bandera. 1=Disparo disponible
        private ISingleCollisionQuery<Pill> pillCollision;

        // id2;                    // Identificador general
        // id3;                    // Identificador de caracter general

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
            this.pillCollision = h.GetCollision2D<Pill>();
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
                if ((Input.Button03() || (demo && Rand.Next(0, 10) == 0)) && View.Graph == 4)
                {
                    // Si no ha disparado esta vez
                    if (fuego_preparado)
                    {
                        // laser(x-16,y-8);    // Dispara los lasers...
                        // laser(x+16,y-8);
                        if (!demo)
                        {
                            // Y lo deja preparado para disparar de uno en uno
                            fuego_preparado = false;
                        }
                    }
                }
                else
                {
                    // Permite volver a disparar
                    fuego_preparado = true;
                }

                //    // Comprueba cuando coges una p¡ldora de bonos                
                if (pillCollision.OnCollision(out Pill pill))
                {
                    pill.Pick();                    
                //        if (id2.size==100)                  // Comprueba que no se halla cogido antes
                //            sound(s_pildora,80,256);
                //            puntuacion+=50;                 // Suma puntos
                //            switch (id2.graph);             // Comprueba que tipo de pildora es
                //                CASE 200:                   // P¡ldora de extension
                //                    if (tamanio_raqueta<8)   // Comprueba que extension tiene actualmente
                //                        tamanio_raqueta+=4;
                //                    }
                //                    if (tamanio_raqueta==4)  // Coloca el grafico dependiendo del tamanio
                //                        graph=3;
                //                     } else {
                //                        graph=4;
                //                    }
                //                    // Deja que la bola se mueva si es que estaba en modo pegamento
                //                    id3=get_id(TYPE bola);
                //                    While(() =>  (id3)
                //                        id3.parado=0;
                //                        id3=get_id(TYPE bola);
                //                    }
                //                }
                //                CASE 201:               // P¡ldora pegamento
                //                    tamanio_raqueta=4;   // Pone el tamanio en normal
                //                    graph=5;            // Selecciona el grafico necesario
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
                //                CASE 204:               // P¡ldora de controles invertidos
                //                    camb_dir=camb_dir*-1;
                //                }
                //                CASE 205:               // Reduce la raqueta
                //                    if (tamanio_raqueta>0)   // Reduce el tamanio de la raqueta si se puede
                //                        tamanio_raqueta-=4;
                //                    }
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
                //                CASE 207:                   // P¡ldora de superbola
                //                    rebota=rebota ^ 1;    // Quita el rebote con los ladrillos
                //                    id3=get_id(TYPE bola);
                //                    While(() =>  (id3)
                //                        id3.graph=1+(1-rebota)*8;    // Cambia el grafico
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
                    Location.X+=incX; 
                }

#if DEBUG || SHOWFPS                
                Location.X = Find.All<Ball>().OrderByDescending(p => p.Location.Y).FirstOrDefault().IfNotNull(b => b.Location.X);
#endif

                // Limites de la pantalla segun el tamanio
                if (Location.X < 24f + tamanio_raqueta)
                {
                    Location.X = 24f + tamanio_raqueta;
                    incX = 0f;
                }
                if (Location.X > 249f - tamanio_raqueta)
                {
                    Location.X = 249f - tamanio_raqueta;
                    incX = 0f;
                }
                Frame();
            });
        }

        /// <summary>
        /// Acción a realizar cuando se agarra una píldora violeta que invierte las direcciones del teclado.
        /// </summary>
        internal void Backwards() => backwardsControl = backwardsControl * -1;        

        /// <summary>
        /// Indica que agarró una píldora que genera múltiples bolas.
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
