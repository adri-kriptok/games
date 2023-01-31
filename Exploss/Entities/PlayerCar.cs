using Exploss.Entities.Bubbles;
using Exploss.Scenes;
using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Div.Entities.Tutorial;
using Kriptok.Div.Views.Cars;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Div;
using System.Windows.Forms;

namespace Exploss.Entities
{
    public class PlayerCar : EntityBase<Car08RedView>
    {
        private ISingleCollisionQuery<Obstacle> obstacleCollision;
        private ISingleCollisionQuery<BubbleBase> bubbleCollision;

        private ISoundHandler lostSound, liveSound;

        private int direction;

        public PlayerCar() : base(new Car08RedView()
        {
            ScaleX = 2f,
            ScaleY = 2f
        })
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;

            obstacleCollision = h.GetCollision2D<Obstacle>();
            bubbleCollision = h.GetCollision2D<BubbleBase>();

            lostSound = h.Audio.GetSoundHandler(DivResources.Sound("MusEfect.MUSICA2.WAV"));
            liveSound = h.Audio.GetWaveHandler(Assembly, "EXPLOSS1.WAV");

            View.Graph = 0;
            Location.Z = -2f;
            Location.X = 65f;
            Location.Y = 65f;
        }

        protected override void OnFrame()
        {
            var ableToShoot = true;
            var speed = 9f;

            // Me fijo si puede disparar.
            if (obstacleCollision.OnCollision())
            {
                ableToShoot = false;
                speed = 2f;
            }

            //Se realizara el bucle infinito que viene a continuacion hasta que
            //maten al coche (colisione con una bola)

            //En funcion de las teclas pulsadas el coche tendra un grafico distinto
            //ademas se determina la direccion que tiene el coche por si se realiza
            //un disparo, para las teclas habra que comprobar si son diagonales o
            //son teclas simples

            if (Input.Up() && (!(Input.Left() || Input.Right())) && Location.Y > 65)
            {
                View.Graph = 0;
                Location.Y -= speed;
                direction = 1;
            }
            if (Input.Down() && (!(Input.Left() || Input.Right())) && Location.Y < 325)
            {
                View.Graph = 2;
                Location.Y += speed;
                direction = 2;
            }
            if (Input.Right() && (!(Input.Up() || Input.Down())) && Location.X < 575)
            {
                View.Graph = 1;
                Location.X += speed;
                direction = 3;
            }
            if (Input.Left() && (!(Input.Up() || Input.Down())) && Location.X > 65)
            {
                View.Graph = 3;
                Location.X -= speed;
                direction = 4;
            }
            if (Input.Up() && Input.Left() && Location.Y > 65 && Location.X > 65)
            {
                View.Graph = 7;
                Location.X -= speed;
                Location.Y -= speed;
                direction = 5;
            }
            if (Input.Up() && Input.Right() && Location.Y > 65 && Location.X < 575)
            {
                View.Graph = 4;
                Location.X += speed;
                Location.Y -= speed;
                direction = 6;
            }
            if (Input.Down() && Input.Left() && Location.Y < 325 && Location.X > 65)
            {
                View.Graph = 6;
                Location.X -= speed;
                Location.Y += speed;
                direction = 7;
            }
            if (Input.Down() && Input.Right() && Location.Y < 325 && Location.X < 575)
            {
                View.Graph = 5;
                Location.X += speed;
                Location.Y += speed;
                direction = 8;
            }

            //Si se pulsa la barra espaciadora y el flag PUEDE_DISPARAR esta a 1
            //(el coche no esta encima de un bloque marron) se genera un disparo
            //ademas se comprueba que no se deja pulsada la tecla de disparo con
            //el flag DISPAR&&O

            if (ableToShoot)
            {
#if DEBUG
                if (Input.Key(Keys.Space))
#else
                if (Input.KeyPressed(Keys.Space))
#endif
                {                    
                    Add(new Shot(Location.X, Location.Y, direction));                    
                }                
            }

            // Si se pulsa la tecla ENTER se genera una bomba,
            // siempre y cuando haya bombas disponibles.
            if (Input.KeyPressed(Keys.Enter) && (Global.CurrentBombs > 0))
            {
                Global.CurrentBombs--;
                Add(new Bomb(Location.X, Location.Y));
            }

#if DEBUG
            if (Global.BlocksPercentage >= Consts.RequiredPercentage || Input.Key(Keys.F6))
#else
            if (Global.BlocksPercentage >= Consts.RequiredPercentage)
#endif
            {
                Scene.SendMessage(GameMessages.Clear);
                Sleep();
                return;
            }

            if (Global.Score > Global.NextLiveScore)
            {
                Global.NextLiveScore += Consts.ScoreLivesInterval;
                liveSound.Play();
                Global.CurrentLives += 1;
            }

            if (Input.Key(Keys.Escape))
            {
                Scene.SendMessage(GameMessages.BackToMenu);
                Die();
                return;
            }

            // Comprobamos si colisiona con algun tipo de bola.
            if (bubbleCollision.OnCollision())
            {
                lostSound.Play();
                Add(new PlayerExplosion(Location));

                Scene.SendMessage(GameMessages.PlayerDied);

                Die();
            }
        }

        private class PlayerExplosion : Explosion
        {
            public PlayerExplosion(Vector3F location)
            {
                Location = location;
                View.ScaleX = 0.5f;
                View.ScaleY = 0.5f;
            }
        }
    }
}