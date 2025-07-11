using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Views.Sprites;
using Kriptok.Entities.Base;
using Kriptok.Audio;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class EnemyRocketLauncher : EnemyBase
    {
        private static readonly int[] indexes = new int[32] { 
            0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 6, 5, 4, 3, 2, 1, 0, 0, 0 };
        private ISoundHandler shieldSound;

        public EnemyRocketLauncher(int tray, int agresividad, int n_grupo) 
            : base(new IndexedSpriteView(typeof(EnemyRocketLauncher).Assembly, "Assets.Images.Ships.EnemyRocketLauncher.png", 3, 3), tray, agresividad, n_grupo)
        {
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            this.shieldSound = h.Audio.GetWaveHandler("Assets.Sounds.Shield.wav");
        }

        internal override void Shoot()
        {            
            if (View.Graph != 7)
            {                
                Add(new ShotRocket(Location.X, Location.Y, GetAngle2D(Global.PlayerShip)));
            }
        }

        internal override void HitByShip()
        {
            if (IsAlive() && (View != null && View.Graph != 0))
            {
                // Cuando está cerrado, no le afecta.
                base.HitByShip();
            }            
        }

        protected override void HitByShot(int tocado)
        {
            base.HitByShot(tocado);

            // Enemigo es invulnerable
            if (View.Graph == 0)         
            {
                View.Graph = 8;
                
                shieldSound.Play();
            }            
        }

        protected override int[] GetIndexes() => indexes;

        protected override int GetScore() => 300;

        protected override int GetEnergy() => 4;
    }
}
