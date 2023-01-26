using Kriptok.Audio;
using Kriptok.Drawing;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Regions.Screen;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;
using Pacoman.Scenes;
using Pacoman.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pacoman.Entities
{
    public class Paco : ProcessBase<PacomanClippedView>
    {
        private readonly int level;

        /// <summary>
        /// Referencia al fondo de la pantalla.
        /// </summary>
        private readonly ScreenRegionBackground bg = null;

        /// <summary>
        /// Mapa de durezas.
        /// </summary>
        private readonly FastBitmap8 hm = null;

        private int imagen;             // Contador de imagenes
        private int velocidad_paco = 2;   // Velocidad de pacoman

        private int xIncr = 0;           // Coordenadas relativas
        private int yIncr = 0;

        /// <summary>
        /// Identificador de fantasma
        /// </summary>
        private Ghost eatenBy;

        /// <summary>
        /// Tabla de animacion.
        /// </summary>
        private readonly int[] graphs = new int[] { 0, 1, 2, 1 };

        /// <summary>
        /// Modificador de pasos según la dirección.
        /// </summary>
        private int stepDirection = 0;

        /// <summary>
        /// Contador de animacion.
        /// </summary>
        private int step = 0;

        /// <summary>
        /// Contador de fantasmas comidos.
        /// </summary>
        private int puntos_por_comido = 200;

        /// <summary>
        /// Controlador de colisiones con fantasmas.
        /// </summary>
        private ISingleCollisionQuery<Ghost> ghostCollision;

        /// <summary>
        /// Sonidos que hace el jugador.
        /// </summary>
        private ISoundHandler eatDotWave, eatBlinkerWave, eatGhostWave, deathWave;

        public Paco(ScreenRegionBackground background, FastBitmap8 hardnesses, int level)
            : base(new PacomanClippedView(new IndexedSpriteView(typeof(Life).Assembly, "Images.Player.png", 7, 2)))
        {
            this.level = level;
            bg = background;
            hm = hardnesses;
        }

        public int X { get { return Location.X.Floor(); } set { Location.X = value; } }

        public int Y { get { return Location.Y.Floor(); } set { Location.Y = value; } }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision2DEllipse();
            this.ghostCollision = h.GetCollision2D<Ghost>();

            this.eatDotWave = h.Audio.GetWaveHandler("Assets.Sounds.Eat.wav");
            this.eatBlinkerWave = h.Audio.GetWaveHandler("Assets.Sounds.EatBlinker.wav");
            this.eatGhostWave = h.Audio.GetWaveHandler("Assets.Sounds.EatGhost.wav");
            this.deathWave = h.Audio.GetWaveHandler("Assets.Sounds.Death.wav");
        }

        protected override void OnBegin()
        {
            // Coge el identificador del proceso
            Global.Player = this;

            // Asigna el grafico y las coordenadas
            View.Graph = 0;
            Location.X = 320;
            Location.Y = 364;

            Loop(() =>
            {
                // Lee teclas
                // Cambia velocidad si se pulsa la barra espaciadora
                if (Input.Escape())
                {
                    Global.LiveCount = -1;
                    Scene.SendMessage(GameMessage.BackToIntro);
                    Die();
                    return;
                }

                // Comprueba si se pulso el cursor de la derecha y puede ir en esa direccion
                if (Input.Right() && CanMove(X + 2, Y))
                {
                    stepDirection = 0;
                    View.Flip = FlipEnum.None;
                    xIncr = 2;           // Pone los incrementos vertical y horizontal
                    yIncr = 0;
                }

                // Comprueba si se pulso el cursor de la izquierda y puede moverse en esa direccion
                if (Input.Left() && CanMove(X - 2, Y))
                {
                    stepDirection = 0;
                    View.Flip = FlipEnum.FlipX;
                    xIncr = -2;
                    yIncr = 0;
                }

                // Comprueba si se pulso el cursor de abajo y puede avanzar
                if (Input.Down() && CanMove(X, Y + 2))
                {
                    stepDirection = 3;
                    View.Flip = FlipEnum.None;
                    yIncr = 2;
                    xIncr = 0;
                }

                // Comprueba si se pulso el cursor de arriba y puede avanzar
                if (Input.Up() && CanMove(X, Y - 2))
                {
                    stepDirection = 3;
                    View.Flip = FlipEnum.FlipY;
                    yIncr = -2;
                    xIncr = 0;
                }

                // Comprueba caminos en la direccion horizontal
                if (CanMove(Location.X.Floor() + xIncr, Y))
                {
                    X += xIncr;  // Si se pude mover se mueve
                }
                else
                {
                    xIncr = 0; // Detiene el movimiento
                    step = 1; //  Y reinicia la animacion
                }

                // Comprueba caminos en la direccion horizontal
                if (CanMove(X, Y + yIncr))
                {
                    Y += yIncr;  // Si se pude mover se mueve
                }
                else
                {
                    yIncr = 0; // Detiene el movimiento
                    step = 1; //  Y reinicia la animacion
                }

                // Selecciona graficos.
                View.Graph = stepDirection + graphs[step];

                // Unicamente muestra la pantalla a la velocidad de paco
                if (imagen >= velocidad_paco)
                {
                    Frame();

                    imagen = 0;

                    // Comprueba si ha colisionado con un fantasma.                    
                    if (ghostCollision.OnCollision(out eatenBy))
                    {
                        // Mira si realmente el fantasma esta cerca
                        if (Math.Abs(X - eatenBy.X) > 10 || Math.Abs(Y - eatenBy.Y) > 10)
                        {
                            eatenBy = null;   // Hace como si no se hubieran tocado
                        }
                    }

                    // Actualiza la animacion del grafico
                    if (xIncr != 0 || yIncr != 0)
                    {
                        step = (step + 1) % 4;
                    }
                }

                // Incrementa el contador de imagenes
                imagen++;

                // Verifica si pacoman ha salido por los lados
                // Hace que aparezca por el otro lado                
                if (X <= 95) X += 450;
                if (X >= 546) X -= 450;

                // Pacoman ha comido un punto
                if (Sample(X, Y) == 14)
                {
                    // Reproduce el sonido.
                    eatDotWave.Play();                    

                    Global.Balls++; //     Incrementa el contador de puntos comidos
                    Global.Score += 10; // Incrementa la puntuacion

                    // Borra el punto del mapa de durezas                                        
                    bg.BlitRect(Color.Black, X - 5, Y - 5, 10, 10);

                    // Incrementa el contador imagenes                    
                    imagen++;
                }
                else if (Sample(X, Y) == 10)
                {
                    // Pacoman ha comido un punto grande.

                    // Reproduce el sonido
                    eatBlinkerWave.Play();                     

                    Global.Balls++; //     Incrementa el numero de puntos comidos
                    Global.Score += 50; // Incrementa la puntuacion

                    // Reinicia la variable que guarda
                    // los puntos que se dan cuando se come a un fantasma
                    puntos_por_comido = 0;

                    // Quita el punto del mapa de durezas.                              
                    bg.BlitRect(Color.Black, X - 10, Y - 10, 20, 20);

                    // Pone a todos los fantasmas en estado de ser comidos.                    
                    foreach (var ghost in FindOtherEntities<Ghost>())
                    {
                        ghost.state = Global.CapsuleTimes[level];
                    }
                }

                // Me fijo si terminó el nivel.
                CheckScore();

                // Comprueba si ha tocado un fantasma
                if (eatenBy != null)
                {
                    // Pacoman se come al fantasma.
                    if (eatenBy.state > 0)
                    {
                        // Realiza el sonido                                            
                        eatGhostWave.Play();

                        // Congela a todos los fantasmas                        
                        foreach (var ghost in FindOtherEntities<Ghost>())
                        {
                            ghost.Sleep();
                        }

                        // Quita el grafico
                        View.Alpha = 0f;
                        eatenBy.View.Alpha = 0f;
                        Add(new EatScore(eatenBy.Location, 0, puntos_por_comido));

                        // Espera a que se den 15 imagenes
                        Frame(EatScore.Frames);

                        // Vuelve a todos los fantasma al estado en que estaban                        
                        foreach (var ghost in FindOtherEntities<Ghost>())
                        {
                            ghost.Wakeup();
                        }

                        // Recupera el grafico de paco
                        View.Alpha = 1f;

                        if (IsAlive())
                        {
                            eatenBy.Eaten();

                            // Da la puntuacion segun los fantasmas comidos
                            switch (puntos_por_comido)
                            {
                                case 0: Global.Score += 200; break;
                                case 1: Global.Score += 400; break;
                                case 2: Global.Score += 800; break;
                                case 3: Global.Score += 1600; break;
                                case 4: Global.Score += 3200; break;
                            }

                            // Incrementa para la proxima vez que coma
                            puntos_por_comido++;
                        }
                    }
                    else
                    {
                        // Fantasma se come a pacoman.

                        // Congela los procesos de todos los fantasmas                        
                        foreach (var ghost in FindOtherEntities<Ghost>())
                        {
                            ghost.Sleep();
                        }

                        // Espera a que se den 15 imagenes
                        Frame(15);

                        // Elimina los procesos de fantasmas, ojos y frutas
                        Find.All<Ghost>().ForEach(p => p.Die());
                        Find.All<Eyes>().ForEach(p => p.Die());
                        Find.All<Fruit>().ForEach(p => p.Die());

                        // Pone el grafico sin ningun espejado
                        View.Flip = FlipEnum.None;

                        // Realiza el sonido                                            
                        deathWave.Play();

                        // Hace la animacion de ser comido
                        for (var i = 6; i <= 13; i++)
                        {
                            View.Graph = i;
                            Frame(4);
                        }

                        // Espera 8 imagenes de pantalla
                        Frame(8);

                        // Comprueba si le quedan vidas
                        if (Global.LiveCount > 0)
                        {
                            Global.Lives[Global.LiveCount].Die();
                        }

                        // Quita una vida
                        Global.LiveCount--;

                        // Mata el proceso y vuelve a empezar.
                        Scene.SendMessage(GameMessage.Killed);
                        Die();
                        return;
                    }

                    // Borra cualquier colision detectada que hubiera
                    eatenBy = null;
                }
            });
        }

        private void CheckScore()
        {
#if DEBUG
            // Siguiente nivel
            if (Global.Balls == 246 || Input.Key(Keys.F6))
#else
            if (Global.Balls == 246)
#endif
            {

                View.Graph = 0;
                Sleep();
                Scene.SendMessage(GameMessage.NextLevel);
                Loop(() =>
                {
                    Frame();
                });                
            }

            // Pone las frutas cada vez se comen 100 o 200 puntos (cocos)
            if (Global.Balls == 100 || Global.Balls == 200)
            {
                Add(new Fruit(level));
                Global.Balls++;
            }

            // Da una vida a los 10000 puntos
            if (Global.Score >= 10000 && Global.PreviewsScore < 10000)
            {
                Global.Lives[Global.LiveCount] = Add(new Life(Global.LiveCount * 26 + 552));
                ++Global.LiveCount;
            }

            // Da otra vida a los 50000 puntos
            if (Global.Score >= 50000 && Global.PreviewsScore < 50000)
            {
                Global.Lives[Global.LiveCount] = Add(new Life(Global.LiveCount * 26 + 552));
                ++Global.LiveCount;
            }

            // Actualiza puntuacion antigua, necesaria para dar vidas solo una vez
            Global.PreviewsScore = Global.Score;
        }

        /// <summary>
        /// Indica si el jugador se puede mover en esa dirección.
        /// </summary>        
        private bool CanMove(int x, int y) => SampleHardMap(x, y).In((byte)10, (byte)12, (byte)14);

        private byte Sample(int x, int y)
        {
            // Primero me fijo contra el fondo si estoy sobre un punto.
            if (bg.Sample(x, y) == 0xFF000000u)
            {
                // Este valor no existe en el fondo.
                return 0;
            }

            return SampleHardMap(x, y);
        }

        private byte SampleHardMap(int x, int y)
        {
            // Comprueba si son los lados de la pantalla
            if ((x < 105 || x > 534) && (y == 225 || y == 226))
            {
                // Devuelve un color de camino.
                // Esto es para que pueda atravesar los teletransportadores.
                return 12;
            }

            // Devuelve el color del mapa de durezas                        
            return hm.Sample((ushort)((x - 105) / 2), (ushort)((y - 1) / 2), 0);
        }
    }
}
