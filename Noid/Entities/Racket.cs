using Kriptok.Common;
using Kriptok.Extensions;
using Noid.Entities.Pills;
using Noid.Scenes;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using Kriptok.Views.Sprites.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kriptok.Audio;

namespace Noid.Entities
{
    class Racket : EntityBase<RacketView>
    {
        /// <summary>
        /// Máxima velocidad de movimiento.
        /// </summary>
        public const int MaxSpeed = 5;

        /// <summary>
        /// Velocidad de movimiento de la raqueta.
        /// </summary>
        private const float speed = 1f;

        /// <summary>
        /// Velocidad de des-aceleración de la raqueta.
        /// </summary>
        private const float aceleration = speed / 2f;

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
        private bool laser = false;

        /// <summary>
        /// Indica si está listo para disparar un laser.
        /// </summary>
        private bool readyToFire = true;

        /// <summary>
        /// Control de colisiones con píldoras.
        /// </summary>
        private ISingleCollisionQuery<PillBase> pillCollision;

        /// <summary>
        /// Incremento X.
        /// </summary>
        private float incX = 0f;
        private ISoundHandler laserSound;

        public Racket(bool demo) : base(new RacketView())
        {
            this.demo = demo;

            // Coordenadas iniciales.
            Location.X = 140;
            Location.Y = 188;
        }

        internal void SetBall(Ball ball)
        {
            this.ball = ball;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
        
            h.CollisionType = Collision2DTypeEnum.Rectangle;
            this.pillCollision = h.GetCollision2D<PillBase>();

            // Empieza con una raqueta normal
            View.Graph = 0;

            // Coordenadas iniciales
            Location.X = 140;
            Location.Y = 188;

            this.laserSound = h.Audio.GetSoundHandler(Sounds.LaserSound);
        }

        protected override void OnFrame()
        {
            // Espera si no hay bola
            if (ball == null)
            {
                return;
            }

            // El jugador controla la raqueta
            if (demo)
            {
                // El ordenador controla la raqueta (modo demo)
                if ((Location.X > 7) && (Location.X < 310))
                {
                    Location.X = ball.Location.X;
                }

                if (Input.KeyPressed())
                {
                    Scene.SendMessage(LevelSceneMessages.StartGame);
                    ball.Die();
                    Die();
                    return;
                }
            }
            else
            {
                if (Input.Key(Keys.Escape))
                {
                    Scene.SendMessage(LevelSceneMessages.BackToIntro);
                    ball.Die();
                    Die();
                    return;
                }

                // Comprueba la pulsacion de las teclas de los cursores
                if (Input.Left() && incX > -MaxSpeed)
                {
                    incX -= speed * aceleration * backwardsControl;  // Acelera
                }
                else if (Input.Right() && incX < MaxSpeed)
                {
                    incX += speed * aceleration * backwardsControl;  // Acelera
                }
                else
                {
                    // Ninguna tecla pulsada
                    if (incX > 0)
                    {
                        incX -= aceleration * backwardsControl;
                    }
                    else if (incX < 0)
                    {
                        // Frena la raqueta
                        incX += aceleration * backwardsControl;
                    }
                }
            }            

            // Comprueba si se pulsa las teclas de disparo y se tiene la modalidad de disparo
            if (laser && ((demo && Rand.Next(0, 15) == 0) || Input.Button03()))
            {
                // Si no ha disparado esta vez
                if (readyToFire)
                {
                    // Y realiza sonido.                        
                    laserSound.Play();

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
            }

            // Si no esta en demo mueve la paleta
            if (!demo)
            {
                Location.X += incX;
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
            : base(Resource.Get(typeof(RacketView).Assembly, "Assets.Images.PadNormal.png"), 1, 1)
        {
            Add(typeof(RacketView).Assembly, "Assets.Images.PadSmall.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadLarge.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadGlue.png", 1, 1);
            Add(typeof(RacketView).Assembly, "Assets.Images.PadLaser.png", 1, 1);
        }
    }
}
