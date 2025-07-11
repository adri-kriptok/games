using Kriptok.Games.BlastemUp.Common;
using Kriptok.Games.BlastemUp.Player;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using static Kriptok.Games.BlastemUp.Enemies.EnemyTrajectory;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public abstract class EnemyBase : ProcessBase<IndexedSpriteView>
    {
        /// <summary>
        /// Energia inicial de la nave.
        /// </summary>
        internal protected int energy;

        /// <summary>
        /// Identificador para colisiones.
        /// </summary>
        internal protected EntityBase collisionObject;

        private ISingleCollisionQuery<ShieldBall> shieldCollision;
        private ISingleCollisionQuery<PlayerShot> shotCollision;
        
        /// <summary>
        /// Consulta para saber si el objeto salió de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        private readonly int tray;
        private readonly int agresividad;
        private readonly int groupId;

        private readonly int n_animacion_s;  // Numero de pasos en animacion
        private float animacion = 0f;    // Contador de animaciones

        /// <summary>
        /// Contador de el numero de pasos en la seccion actual
        /// </summary>
        private float sectionCounter = 0;

        private float speedX;    // Velocidad
        private float speedY;

        private float accX;  // Aceleracion
        private float accY;

        private readonly int sections = 0;   // Numero de seccion
        private int currentSection = 0;   // Seccion actual
        private readonly int puntos;             // Puntos por matar a un enemigo
        private int tocado;             // Impactos recibidos

        private int imagenes;   // Numero de pasos en la seccion actual
        private int muerto = 0;   // Bandera. 1=muerto

        private readonly int[] animacion_enemigo;

        public EnemyBase(IndexedSpriteView view, int tray, int agresividad, int groupId) 
            : base(view)
        {
            this.tray = tray;
            this.agresividad = agresividad;
            this.groupId = groupId;

            animacion_enemigo = GetIndexes();

            // Selecciona el bucle de animacion y el grafico ha asignar
            n_animacion_s = animacion_enemigo.Length;
            View.Graph = animacion_enemigo[(int)animacion];

            // Elige los puntos que dara el enemigo
            puntos = GetScore();

            // Elige la energ¡a del enemigo            
            energy = GetEnergy();

            // Elige la seccion y el numero de imagenes de por segundo
            sections = trayectoria[tray].Length;
            UpdateSection(tray, currentSection);

            Location.X = trayectoria[tray].InitX;      // Selecciona las coordenadas iniciales
            Location.Y = trayectoria[tray].InitY;

            Global.vivos[groupId]++;               // Incremento el numero de enemigos del grupo

        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.shieldCollision = h.GetCollision2D<ShieldBall>();
            this.shotCollision = h.GetCollision2D<PlayerShot>();
            this.outOfScreen = h.GetOutOfScreenQuery();
        }

        protected abstract int GetEnergy();

        protected abstract int GetScore();

        /// <summary>
        /// Índices de animación.
        /// </summary>        
        protected abstract int[] GetIndexes();

        protected override void OnBegin()
        {
            Loop(() =>
            {               
                tocado = 0;   // Reinicia la variable que controla si se ha tocado a un enemigo

                // Comprueba si el enemigo choca con el escudo de bolas
                if (energy > tocado && shieldCollision.OnCollision())
                {
                    Add(new Explosion(Rand.Next(0, 2), Rand.Next(0, 19) + 30, Location.X, Location.Y));                    
                    HitByShield();                    
                }

                // Comprueba si el enemigo choca con una bala
                if (energy > tocado && shotCollision.OnCollision(out PlayerShot shot))
                {
                    Add(new Explosion(Rand.Next(0, 2), Rand.Next(0, 19) + 20, Location.X, Location.Y));

                    // Quita la bala                    
                    shot.Die();

                    // El enemigo a sido tocado
                    tocado++;                       
                }

                if (tocado != 0)
                {
                    HitByShot(tocado);
                }

                // Comprueba si el enemigo esta muerto
                if (energy <= 0)
                {
                    muerto = 1;
                    
                    Explode();
    
                    Global.Score += puntos;  // Puntos para el jugador
                }

                // Crea un valor aleatorio y lo comprueba con la agresividad
                if (Rand.Next(0, 199) < agresividad)
                {
                    Shoot();
                }

                Frame();
                
                sectionCounter += Consts.SpeedModifier;

                // Comprueba si ha desaparecido este proceso de pantalla o esta muerto
                if (outOfScreen.Result.GetValueOrDefault(false) || (muerto != 0))
                {
                    if (muerto == 0)
                    {
                        Global.no_bonus[groupId] = 1;
                    }
                    Global.vivos[groupId]--;       // Resta un enemigo
                    if ((Global.vivos[groupId] == 0) && (Global.no_bonus[groupId] == 0))
                    {
                        // Seleccion la posicion de los bonus en la pantalla
                        Global.bonus_x = Location.X;
                        Global.bonus_y = Location.Y;
                    }

                    Die();
                    return;               // El proceso acaba, saliendo con RETURN
                }
                else
                {
                    // Bucle principal
                    // Movimiento enemigo
                    if (sectionCounter >= imagenes)
                    {
                        currentSection++;
                        sectionCounter = 0f;
                        if (currentSection >= sections)
                        {
                            currentSection = 0;
                        }

                        UpdateSection(tray, currentSection);
                    }

                    speedX += accX;
                    speedY += accY;
                    Location.X += speedX;
                    Location.Y += speedY;

                    // Anima el grafico
                    animacion += Consts.SpeedModifier;
                    if (animacion >= n_animacion_s)
                    {
                        animacion = 0f;
                    }

                    View.Graph = animacion_enemigo[(int)animacion];

                }
            });
        }

        /// <summary>
        /// El enemigo ha sido golpeado por un disparo del jugador.
        /// </summary>
        protected virtual void HitByShot(int tocado)
        {
            energy -= tocado;
        }

        /// <summary>
        /// El enemigo ha sido tocado por el escudo.
        /// </summary>
        protected virtual void HitByShield()
        {
            tocado++;
        }

        /// <summary>
        /// Tocado por la nave protagonista.
        /// </summary>
        internal virtual void HitByShip()
        {
            energy--;            
        }

        internal virtual void Explode()
        {
            Add(new Explosion(Rand.Next(0, 2), 80, Location.X, Location.Y));
        }

        internal abstract void Shoot();

        private void UpdateSection(int tray, int actual_seccion)
        {
            var sect = trayectoria[tray].Items[actual_seccion];

            imagenes = sect.imagener_por_seccion;
            
            speedX = sect.Speed.X;   // Velocidad de seccion x
            speedY = sect.Speed.Y;   // Velocidad de seccion y

            accX = sect.Acceleration.X; // Aceleracion de seccion x
            accY = sect.Acceleration.Y; // Aceleracion de seccion y
        }

        internal static EntityBase New(int type, int tray, int agresividad, int n_grupo)
        {
            // Tipo de movimiento y grafico del disparo
            switch (type)
            {
                case 0: return new Enemy0(tray, agresividad, n_grupo);
                case 1: return new Enemy1(tray, agresividad, n_grupo);
                case 2: return new Enemy2(tray, agresividad, n_grupo);
                case 3: return new EnemyRocketLauncher(tray, agresividad, n_grupo);
                case 4: return new EnemyMine(n_grupo);
                case 5: return new Enemy5(tray, agresividad, n_grupo);
                default: return null;
            }
        }

        public static EnemyTrajectory[] trayectoria = new EnemyTrajectory[]
        {
            new EnemyTrajectory(630,240,
                new EnemyTrajectoryItem(12,0,-8,0,0),
                new EnemyTrajectoryItem(24,0,8,0,0),
                new EnemyTrajectoryItem(12,0,-8,0,0),
                new EnemyTrajectoryItem(15,-32,-15,0,1),
                new EnemyTrajectoryItem(30,16,0,0,0),
                new EnemyTrajectoryItem(12,0,8,0,0),
                new EnemyTrajectoryItem(15,-32,15,0, -1),
                new EnemyTrajectoryItem(30,16,0,0,0),
                new EnemyTrajectoryItem(12,0,-8,0,0),
                new EnemyTrajectoryItem(30,-16,0,0,0),
                new EnemyTrajectoryItem(30,16,0,0,0),
                new EnemyTrajectoryItem(15,-32,15,0,-1),
                new EnemyTrajectoryItem(24,0,-8,0,0),
                new EnemyTrajectoryItem(30,16,0,0,0),
                new EnemyTrajectoryItem(15,-32,15,0,-1),
                new EnemyTrajectoryItem(30,16,0,0,0),
                new EnemyTrajectoryItem(15,-32,-15,0,1),
                new EnemyTrajectoryItem(12,0,8,0,0), 
                new EnemyTrajectoryItem(30,16,0,0,0)),

            new EnemyTrajectory(650,280,
                new EnemyTrajectoryItem(80,-35,0,1,0)),

            new EnemyTrajectory(300,20,
                new EnemyTrajectoryItem(18,1,1,1,1),
                new EnemyTrajectoryItem(30,15,15,-5,1)),

            new EnemyTrajectory(0,60,
                new EnemyTrajectoryItem(22,25,0,0,0),
                new EnemyTrajectoryItem(5,0,10,0,0),
                new EnemyTrajectoryItem(20,-25,10,0,0),
                new EnemyTrajectoryItem(20,0,-10,0,0),
                new EnemyTrajectoryItem(20,25,10,0,0),
                new EnemyTrajectoryItem(5,0,10,0,0),
                new EnemyTrajectoryItem(20,-25,0,-1,0)),

            new EnemyTrajectory(630,150,
                new EnemyTrajectoryItem(5,-8,0,0,0),
                new EnemyTrajectoryItem(8,-6,-9,0,0),
                new EnemyTrajectoryItem(16,-7,9,0,0),
                new EnemyTrajectoryItem(16,-6,-9,0,0),
                new EnemyTrajectoryItem(16,-7,9,0,0),
                new EnemyTrajectoryItem(20,-10,0,-2,0)),

            new EnemyTrajectory(630,200,
                new EnemyTrajectoryItem(5,-8,0,0,0),
                new EnemyTrajectoryItem(8,-6,9,0,0),
                new EnemyTrajectoryItem(16,-7,-9,0,0),
                new EnemyTrajectoryItem(16,-6,9,0,0),
                new EnemyTrajectoryItem(16,-7,-9,0,0),
                new EnemyTrajectoryItem(20,-10,0,-2,0)),

            new EnemyTrajectory(630,250,
                new EnemyTrajectoryItem(26,-15,0,0,0),
                new EnemyTrajectoryItem(12,-12,-10,2,0),
                new EnemyTrajectoryItem(12,15,0,0,0),
                new EnemyTrajectoryItem(12,12,10,-2,0),
                new EnemyTrajectoryItem(20,-15,0,-1,0)),

            new EnemyTrajectory(630,110,
                new EnemyTrajectoryItem(10,0,10,0,0),
                new EnemyTrajectoryItem(32,-10,0,0,0),
                new EnemyTrajectoryItem(8,-10,10,0,0),
                new EnemyTrajectoryItem(8,10,10,0,0),
                new EnemyTrajectoryItem(34,10,0,0,0),
                new EnemyTrajectoryItem(8,0,-10,0,0),
                new EnemyTrajectoryItem(40,-10,0,-1,0)),

            new EnemyTrajectory(630,0,
                new EnemyTrajectoryItem(25,-10,25,0,-1),
                new EnemyTrajectoryItem(20,-20,-10,2,0),
                new EnemyTrajectoryItem(20,20,10,-2,0),
                new EnemyTrajectoryItem(20,-20,-10,2,0),
                new EnemyTrajectoryItem(20,20,10,-2,0),
                new EnemyTrajectoryItem(20,-20,-10,2,0),
                new EnemyTrajectoryItem(20,20,10,-2,0),
                new EnemyTrajectoryItem(30,-10,0,-2,0)),

            new EnemyTrajectory(0,220,
                new EnemyTrajectoryItem(22,25,0,0,0),
                new EnemyTrajectoryItem(5,0,10,0,0),
                new EnemyTrajectoryItem(20,-25,10,0,0),
                new EnemyTrajectoryItem(20,0,-10,0,0),
                new EnemyTrajectoryItem(20,25,10,0,0),
                new EnemyTrajectoryItem(5,0,10,0,0),
                new EnemyTrajectoryItem(20,-25,0,-1,0)),

            new EnemyTrajectory(640,140,
                new EnemyTrajectoryItem(8,-10,0,0,3),
                new EnemyTrajectoryItem(16,-10,21,0,-3),
                new EnemyTrajectoryItem(16,-10,-24,0,3),
                new EnemyTrajectoryItem(16,-10,21,0,-3),
                new EnemyTrajectoryItem(16,-10,-24,0,3),
                new EnemyTrajectoryItem(16,-10,21,0,-3))
        };
    }
}
