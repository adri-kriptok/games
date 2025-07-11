using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class Enemy1 : EnemyBase
    {
        private static readonly int[] indexes = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public Enemy1(int tray, int agresividad, int n_grupo) 
            : base(new IndexedSpriteView(typeof(Enemy1).Assembly, "Assets.Images.Ships.Enemy1.png", 4, 4), tray, agresividad, n_grupo)
        {
        }

        protected override int[] GetIndexes() => indexes;

        protected override int GetScore() => 200;

        protected override int GetEnergy() => 2;

        internal override void Shoot()
        {                        
            Add(new Shot1(Location.X, Location.Y, GetAngle2D(Global.PlayerShip)));            
        }
    }
}
