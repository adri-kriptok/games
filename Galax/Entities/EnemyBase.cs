using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using System.Linq;

namespace Galax.Entities
{
    public abstract class EnemyBase<T> : EnemyBase
        where T : ISpriteView
    {
        protected EnemyBase(int index, T view) : base(index, view)
        {
        }

        /// <summary>
        /// Obtiene la vista asociada al objeto.
        /// </summary>
        public new T View => (T)base.View;
    }

    public abstract class EnemyBase : EntityBase
    {
        /// <summary>
        /// Distancia horizontal entre enemigos.
        /// </summary>
        private const int EnemySpaceX = 20;

        /// <summary>
        /// Distancia vertical entre enemigos
        /// </summary>
        private const int EnemySpaceY = 20;

        /// <summary>
        /// Coordenada Y inicial del escuadrón.
        /// </summary>
        private const int SquadronY = 20;

        /// <summary>
        /// Tamanio del escuadron.
        /// </summary>
        private const int SquadWidth = 12;

        /// <summary>
        /// Índice del enemigo.
        /// </summary>
        private readonly int index;

        /// <summary>
        /// Columna en la que empieza el enemgio.
        /// </summary>
        private readonly int col;

        /// <summary>
        /// Fila en la que empieza el enemgio.
        /// </summary>
        private readonly int row;

        /// <summary>
        /// 0=Posicion de escuadron, 1=ataque,
        /// 3=retorna a la posicion de escuadron
        /// </summary>
        private int abajo = 0;
        
        /// <summary>
        /// Velocidad horizontal cuando atacan.
        /// </summary>
        private int velocidad;

        /// <summary>
        /// Consulta de colisión contra los disparos del jugador.
        /// </summary>
        private ISingleCollisionQuery<PlayerMisile> playerMisile;

        /// <summary>
        /// Consulta para saber si salió de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        /// <summary>
        /// Sonido de disparo.
        /// </summary>
        private ISoundHandler laserSound;

        public EnemyBase(int index, IView view) : base(view)
        {
            this.index = index;
            
            // Fila y columna de la posicion en escuadron
            row = index / SquadWidth;    
            col = index % SquadWidth;

            // Selecciona posicion inicial
            Location.X = col * EnemySpaceX + Consts.SquadStartX;
            Location.Y = row * EnemySpaceY + SquadronY;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.playerMisile = h.GetCollision2D<PlayerMisile>();
            this.outOfScreen = h.GetOutOfScreenQuery();

            laserSound = h.Audio.GetWaveHandler("LASER3.WAV");
        }

        protected override void OnFrame()
        {
            if (Global.Player == null)
            {
                return;
            }

            // Puede disparar si vuela bajo.
            if (Location.Y < 130)
            {                
                // Porcentaje de disparo
                Shoot(Rand.Next(0, 1500));
            }

            if (abajo == 1)
            {
                // Ataca la nave
                Location.Y += 2f;

                // Comprueba si ha salido de pantalla
                if (outOfScreen.Result.GetValueOrDefault(false))
                {
                    // Cambia las coordenadas a arriba                      
                    Location.Y = 0;
                    Location.X = col * EnemySpaceX + Global.SquadCurrentX;
                    abajo = 3;
                }

                if (Location.X > Global.Player.Location.X)
                {
                    // Va hacia la nave del jugador
                    if (velocidad > -EnemyType - 2)
                    {
                        velocidad--;
                    }
                }
                else
                {
                    if (velocidad < EnemyType + 2)
                    {
                        velocidad++;
                    }
                }

                Location.X += velocidad;
            }

            if (abajo == 3)
            {
                // Retorna a la posicion de escuadron
                Location.Y++;
                if (Location.Y == row * EnemySpaceY + SquadronY)
                {
                    abajo = 0;
                }

                // Si hay menos enemigos que el nivel por 3 vuelve a bajar
                if (Find.All<EnemyBase>().Count() < Global.Level * 3)
                {
                    abajo = 1;
                }
            }

            if ((abajo == 0) || (abajo == 3))
            {
                // Mueve al escuadron
                var num_aleatorio = Rand.Next(1, 4000);

                // Aleatoriamente y segun el nivel hace que el proceso baje
                if (num_aleatorio < Global.Level * 2)
                {
                    abajo = 1;

                    PlaySound();
                }

                if (Global.MovementDirection == 0)
                {
                    // Si la direccion es hacia la izquierda
                    Location.X -= Consts.SquadronSpeed;

                    // Cuando no se pueda mover a la izquierda cambia de direccion
                    if (Location.X < 20)
                    {
                        Global.ChangeSquadDirectionFlag = true;
                    }

                }
                else
                {
                    // Si la direccion es a la derecha
                    Location.X += Consts.SquadronSpeed;

                    if (Location.X > 300)
                    {
                        Global.ChangeSquadDirectionFlag = true;  // Cambia la direccion
                    }
                }
            }

            if (playerMisile.OnCollision(out PlayerMisile misile))
            {
                // Elimina la nave                    
                misile.KillMisile();

                // Y suma puntos
                Global.Score += 10;

                // Comprueba las vidas extras
                if (Global.Score % 1000 == 0)
                {
                    Global.Lives++;
                }

                // Quita al enemigo de la formacion
                Global.EnemyType[index] = 0;

                Explode();
            }           
        }

        protected abstract int EnemyType { get; }
        protected abstract void PlaySound();

        internal static EnemyBase Create(int i)
        {
            switch (Global.EnemyType[i])
            {
                case 1: return new Enemy1(i);
                case 2: return new Enemy2(i);
                case 3: return new Enemy3(i);
            }
            return null;
        }

        protected abstract void Shoot(int rnd);

        internal void Explode()
        {
            Add(new Explosion(Location.X, Location.Y, 1f));

            // Y elimina el proceso haciendo un sonido y una explosion.            
            laserSound.Play();
            Die();
        }
    }
}
