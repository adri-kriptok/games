using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.BlastemUp.Enemies
{
    /// <summary>
    /// Las minas tienen una trayectoria definida.
    /// No tienen agresividad definida.
    /// </summary>
    public class EnemyMine : EnemyBase
    {
        private static readonly int[] indexes = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };

        public EnemyMine(int groupIndex)
            : base(new IndexedSpriteView(typeof(Enemy5).Assembly, "Assets.Images.Ships.Mine.png", 4, 2), 1, 0, groupIndex)
        {
            Location.Y = Rand.Next(9, 389);
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            h.Audio.GetWaveHandler("Assets.Sounds.Mine.wav").Play();
        }

        protected override int[] GetIndexes() => indexes;

        protected override int GetScore() => 350;

        protected override int GetEnergy() => 5;

        internal override void Shoot()
        {            
        }
    }
}
