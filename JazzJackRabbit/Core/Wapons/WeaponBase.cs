using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.JazzJackRabbit.Entities;
using Kriptok.Views.Gdip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Core
{
    abstract class WeaponBase
    {
        private readonly Jazz jazz;
        private int state = 0;

        public WeaponBase(Jazz jazz)
        {
            this.jazz = jazz;
        }

        internal void Shoot()
        {
            if (state == 0)
            {
                state = 13;
                Shoot(jazz, ShotLocation(), jazz.View.Flip);
            }
        }

        internal int GetState()
        {
            if (state <= 0)
            {
                return 0;
            }
            
            return state--;
        }

        protected abstract void Shoot(Jazz jazz, Vector3F location, FlipEnum flip);

        private Vector3F ShotLocation()
        {
            var loc = jazz.Location;
            var dir = jazz.View.Flip == FlipEnum.None ? 1 : -1;

            loc.X += 20 * dir;            

            var graph = jazz.View.Graph;

            if (graph == 40)
            {
                // Saltando.
                loc.Y += 2;
            }
            else if (graph == 41)
            {
                // Cayendo.
                loc.Y += 9;
            }
            else if (graph.BetweenCloseClose(16, 19))
            {
                // Corriendo.
                loc.Y += 10;
                loc.X += 2 * dir;
            }
            else
            {
                loc.Y += 8;
            }

            return loc;
        }

        /// <summary>
        /// Indica si el arma está lista para disparar.
        /// </summary>        
        internal bool ReadyToShoot() => GetState() == 0;
    }
}
