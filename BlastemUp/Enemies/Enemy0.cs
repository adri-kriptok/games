using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class Enemy0 : EnemyBase
    {
        private static readonly int[] indexes = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public Enemy0(int tray, int agresividad, int n_grupo) 
            : base(new IndexedSpriteView(typeof(Enemy0).Assembly, "Assets.Images.Ships.Enemy0.png", 4, 4), tray, agresividad, n_grupo)
        {
        }

        protected override int[] GetIndexes() => indexes;

        protected override int GetScore() => 150;

        protected override int GetEnergy() => 1;

        internal override void Shoot()
        {            
             Add(new Shot0(Location.X, Location.Y, GetAngle2D(Global.PlayerShip)));            
        }
    }
}
