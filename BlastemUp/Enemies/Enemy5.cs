using Kriptok.Games.BlastemUp.Common;
using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class Enemy5 : EnemyBase
    {
        private float xtiempo = 0;          // Posicion de la explosion del enemigo final
        private float ytiempo = 0;

        public Enemy5(int tray, int agresividad, int n_grupo)
            : base(new IndexedSpriteView(typeof(Enemy5).Assembly, "Assets.Images.Ships.Enemy5.png", 1, 2), tray, agresividad, n_grupo)
        {
        }

        internal override void Shoot()
        {            
            Add(new Shot5(Location.X, Location.Y, GetAngle2D(Global.PlayerShip)));            
        }

        internal override void Explode()
        {
            // Enemigo final muerto

            xtiempo = Location.X; // Guarda las coordenadas para dejar la explosion
            ytiempo = Location.Y;
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    Location.X = xtiempo + Rand.Next(0, 99) - 50;
                    Location.Y = ytiempo + Rand.Next(0, 99) - 50;
                    Add(new Explosion(Rand.Next(0, 2), i * 5 + j + 1, Location.X, Location.Y));
                }

                // Actualiza las coordenadas
                Location.X = xtiempo;
                Location.Y = ytiempo;
                Frame();
            }
        }

        protected override void HitByShield()
        {
            // El enemigo final no es afectado por el escudo.
        }

        internal override void HitByShip()
        {
            // El enemigo final no afecta.
        }

        protected override void HitByShot(int tocado)
        {            
            if (View.Graph == 0)
            {
                View.Graph = 1;
            }

            // El enemigo ha sido tocado
            base.HitByShot(tocado);            
        }

        protected override int[] GetIndexes() => new int[1] { 0 };

        protected override int GetScore() => 800;

        protected override int GetEnergy() => 40;
    }
}
