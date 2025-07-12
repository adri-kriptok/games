using Kriptok.Games.BlastemUp.Scenes;
using Kriptok.Games.BlastemUp.Common;
using Kriptok.Games.BlastemUp.Enemies;
using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using Kriptok.Audio;

namespace Kriptok.Games.BlastemUp.Player
{
	public enum PlayerShipStatusEnum
    {
		Normal = 0, 
	    Invulnerable = 1
    }

    public class PlayerShip : ProcessBase<IndexedSpriteView>
	{
		private const float shootAngle = (float)(Math.PI / 32);

        /// <summary>
        /// Estado de la nave.
        /// </summary>
        internal static PlayerShipStatusEnum PlayerShipStatus;

        /// <summary>
        /// Energia inicial de la nave.
        /// </summary>
        internal protected int energy;

        /// <summary>
        /// Evita el disparo continuo.
        /// </summary>
        private bool disparando = false;

        /// <summary>
        /// Maneja el movimiento y la inercia
        /// </summary>
        private int banco;

        /// <summary>
        /// Incrementos horizontal y verical
        /// </summary>
        private int inc_x = 0, inc_y = 0;

        /// <summary>
        /// Tipo de disparo de la nave (0..disparo_max).
        /// </summary>
        internal int ShotType { get; private set; }

        private ISingleCollisionQuery<EnemyBase> enemyCollision;
        private ISingleCollisionQuery<EnemyShotBase> enemyShotCollision;
        private ISingleCollisionQuery<Bonus> bonusCollision;
		private ISoundHandler bonusSound;
		private ISoundHandler laserSound;
		private bool bLaserSound;

		/// <summary>
		/// Objeto utilizado para crear las letras en la barra de arriba.
		/// </summary>
		private readonly LetterFactory factory;

        internal PlayerShip(LetterFactory factory) : base(new IndexedSpriteView(typeof(PlayerShip).Assembly, "Ships.PlayerShip.png", 4, 4))
		{
			this.factory = factory;
		}

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
			h.CollisionType = Collision2DTypeEnum.Auto;
			this.enemyCollision = h.GetCollision2D<EnemyBase>();
			this.enemyShotCollision = h.GetCollision2D<EnemyShotBase>();
			this.bonusCollision = h.GetCollision2D<Bonus>();

			this.bonusSound = h.Audio.GetWaveHandler("Assets.Sounds.Bonus.wav");
			this.laserSound = h.Audio.GetSoundHandler("Assets.Sounds.Laser0.wav");
		}

        protected override void OnBegin()
		{
			View.Graph = 0;     // Selecciona grafico
			Location.X = 32;    // y coordenadas
			Location.Y = 240 - Consts.PlayRegionMinY;
			Location.Z = -2;
			// Inicializa variables
			PlayerShipStatus = PlayerShipStatusEnum.Normal;
			ShotType = Consts.StartingShootLevel;
			energy = 1;

			Frame();

			Loop(() =>
			{
				// Movimiento de inercia
				if (inc_x > 0) inc_x -= 4;
				if (inc_x < 0) inc_x += 4;
				if (inc_y > 0) inc_y -= 2;
				if (inc_y < 0) inc_y += 2;

				// Control de la nave
				if (Input.Up())
				{
					inc_y = -12;
					banco--;
				}
				if (Input.Down())
				{
					inc_y = 12;
					banco++;
				}
				if (Input.Left())
				{
					inc_x = -24;
				}
				if (Input.Right())
				{
					inc_x = 24;
				}

				// Limitando el banco de inercia
				if (banco > 15) banco -= 16;
				if (banco < -16) banco += 16;

				// Banco de inercia
				if (inc_y > -6 && inc_y < 6)
				{
					if (banco > 0) banco--;
					if (banco < 0) banco++;
				}

				// Cambia el grafico
				if (banco < 0)
				{
					View.Graph = 16 + banco;
				}
				else
				{
					View.Graph = banco;
				}

				// Limita la posicion de la nave
				if ((Location.Y + inc_y) < Consts.MinY)
				{
					Location.Y = Consts.MinY;
					inc_y = 0;
				}
				if ((Location.Y + inc_y) > Consts.MaxY)
				{
					Location.Y = Consts.MaxY;
					inc_y = 0;
				}
				if ((Location.X + inc_x) < 30)
				{
					Location.X = 30;
					inc_x = 0;
				}
				if ((Location.X + inc_x) > 610)
				{
					Location.X = 610;
					inc_x = 0;
				}
				Location.X += inc_x;
				Location.Y += inc_y;

				// Control del disparo
				if (Input.Button01() || Input.Button02() || Input.Button03() || Input.Button04())
				{
					if (disparando == false)
					{
						Shoot(ShotType);
					}
					disparando = !disparando;      // Tecla pulsada
				}
				else
				{
					disparando = false;      // Tecla soltada
					bLaserSound = false;
				}

				// Mira si hay colision con el enemigo.
				if (energy > 0 && enemyCollision.OnCollision(out EnemyBase enemy))
				{
					enemy.HitByShip();

					// Decrementa la energ¡a si no tienes escudo
					if (PlayerShipStatus != PlayerShipStatusEnum.Invulnerable)
					{
						energy--;
					}
				}
				
				// Mira si hay colision con los misiles o el laser enemigo
				if (energy > 0 && enemyShotCollision.OnCollision(out EnemyShotBase enemyShot))
				{
					enemyShot.Die();

					// Decrementa energ¡a si no hay escudo
					if (PlayerShipStatus != PlayerShipStatusEnum.Invulnerable)
					{
						energy--;
					}
				}

				// Se comprueba si la nave ha muerto
				if (energy <= 0)
				{
					Global.LifeCount--;
					if (ShotType > 0)
					{
						ShotType--;
					}

					// La nave desaparece en este momento
					View.Alpha = 0f;

					Add(new Explosion(Rand.Next(0, 2), 100, Location.X, Location.Y));

					if (Global.LifeCount < 0)
					{
						// No hay mas vidas
						Die();
						return;
					}

					if (Global.LifeCount < Consts.MaxLivesOnScreen)
					{
						// Una vida menos en el marcador
						Global.Lives[Global.LifeCount].Die();
					}

					Frame(2);
					InitShield();    // Inicia un escudo
					View.Alpha = 1f; // La nave aparece otra vez
					energy = 1;
				}

				// Mira si se ha recogido bonus
				if (bonusCollision.OnCollision(out Bonus bonus))
				{					
					bonusSound.Play();

					switch (bonus.info)
					{
						case 1: // Bonus
						case 2: // Bonus
						case 3: // Bonus
						case 4: // Bonus
						case 5: // Bonus												
							bonus.AddLetter(factory);
							break;
						case 6: // $
							Global.Score += 1000;
							break;
						case 7: // Atomo
							ShotType++;

							// Si tiene el tipo de disparo maximo, coloca entonces un escudo
							if (ShotType > Consts.MaxShotType)
							{
								ShotType = Consts.MaxShotType;
								InitShield();
							}

							break;
					}

					// Borra los bonus de la pantalla.
					bonus.Die();
				}

				Frame();
			});

			// Borra el escudo            
			foreach (var ball in Find.All<ShieldBall>())
			{
				ball.Die();
			}
		}

		internal void InitShield()
		{
			if (PlayerShipStatus != PlayerShipStatusEnum.Invulnerable)
			{
				// Nave invulnerable
				PlayerShipStatus = PlayerShipStatusEnum.Invulnerable;

				// Crea ocho bolas del escudo
				for (int i = 0; i <= 7; i++)
				{
					Add(new ShieldBall(i));
				}
			}
		}

		public void Shoot(int info)
		{
			if (bLaserSound = !bLaserSound)
			{
				// Realiza sonido			
				laserSound.Play();
			}

			// Comprueba el numero de balas
			switch (info)
			{
				case 0:             
					// Una bala
					Add(new PlayerShot(Location.X, Location.Y, 0f));
					break;
				case 1:             
					// Dos balas
					Add(new PlayerShot(Location.X, Location.Y - 6f, 0f));
					Add(new PlayerShot(Location.X, Location.Y + 6f, 0f));
					break;
				case 2:             
					// Dos balas con otra trayectoria
					Add(new PlayerShot(Location.X, Location.Y - 6f, -shootAngle));
					Add(new PlayerShot(Location.X, Location.Y + 6f, shootAngle));
					break;
				case 3:             
					// Tres balas
					Add(new PlayerShot(Location.X, Location.Y - 8f, -shootAngle));
					Add(new PlayerShot(Location.X, Location.Y, 0f));
					Add(new PlayerShot(Location.X, Location.Y + 8f, shootAngle));
					break;
				case 4:             
					// Cuatro balas
					Add(new PlayerShot(Location.X, Location.Y - 8, -shootAngle));
					Add(new PlayerShot(Location.X, Location.Y - 4, 0));
					Add(new PlayerShot(Location.X, Location.Y + 4, 0));
					Add(new PlayerShot(Location.X, Location.Y + 8, shootAngle));
					break;
			}
		}
	}
}
