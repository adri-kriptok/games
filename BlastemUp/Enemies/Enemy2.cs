using Kriptok.Games.BlastemUp.Enemies.Shots;
using Kriptok.Core;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class Enemy2 : EnemyBase
    {
        private static readonly int[] indexes = new int[20] { 
            0, 1, 2, 3, 4, 4, 5, 6, 7, 8, 8, 7, 6, 5, 4, 4, 3, 2, 1, 0 };

        public Enemy2(int tray, int agresividad, int n_grupo) 
            : base(new IndexedSpriteView(typeof(Enemy2).Assembly, "Assets.Images.Ships.Enemy2.png", 3, 3), tray, agresividad, n_grupo)
        {
        }

        protected override int[] GetIndexes() => indexes;

        protected override int GetScore() => 250;

        protected override int GetEnergy() => 3;

        internal override void Shoot()
        {                        
            Add(new Shot2(Location.X, Location.Y, GetAngle2D(Global.PlayerShip)));            
        }
    }
}
