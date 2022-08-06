using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Galax.Entities;
using Galax.Scenes;
using Kriptok.Views.Sprites;
using System.Windows.Forms;

namespace Galax
{
    public class PlayerShip : EntityBase<SpriteView>
    {
        /// <summary>
        /// Velocidad de la nave.
        /// </summary>
        private int speed = 0;

        private ISingleCollisionQuery<EnemyShotBase> shotCollision;
        private ISingleCollisionQuery<EnemyBase> enemyCollision;

        public PlayerShip() : base(new SpriteView(typeof(PlayerShip).Assembly, "PlayerShip.png"))
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;
            
            Location.X = 160f;
            Location.Y = 188f;
            
            // Sirve para disparar solo un disparo cada vez
            PlayerMisile.Flag = false;

            this.shotCollision = h.GetCollision2D<EnemyShotBase>();
            this.enemyCollision = h.GetCollision2D<EnemyBase>();
        }

        protected override void OnFrame()
        {                        
            // Lee las teclas y mueve la nave
            if (Input.Right() && speed < 6)
            {
                speed += 2;
            }
            else
            {
                if (speed > 0) speed--;
            }

            if (Input.Left() && speed > -6)
            {
                speed -= 2;
            }
            else if (speed < 0)
            {
                speed++;
            }

            Location.X += speed;

            if (Location.X > 310)
            {
                // Limites de la pantalla
                Location.X = 310;
                speed = 0;
            }

            if (Location.X < 10)
            {
                Location.X = 10;
                speed = 0;
            }

            if (Input.KeyPressed(Keys.Space))
            {
#if !DEBUG
                if (PlayerMisile.Flag == false)
                {                              
#endif
                    // Dispara misil
                    // Mira si hay otro disparado                   
                    Audio.PlayWave(GetType().Assembly, "LASER.WAV");
                    PlayerMisile.Flag = true;
                    Add(new PlayerMisile(Location.X, Location.Y - 10f));
#if !DEBUG
                }
#endif
            }

            if (shotCollision.OnCollision(out EnemyShotBase shot))
            {                
                // Crea un proceso tipo explosion
                Add(new Explosion(Location.X, Location.Y, 1f));
                Audio.PlayWave(GetType().Assembly, "LASER3.WAV");

                shot.Die();

                Scene.SendMessage(LevelMessagesEnum.LoseLife);
                Die();
            }
            else if (enemyCollision.OnCollision(out EnemyBase enemy))
            { 
                enemy.Explode();
                Scene.SendMessage(LevelMessagesEnum.LoseLife);
                Die();
            }
        }
    }
}

