using Kriptok.Drawing.Algebra;
using Kriptok.JazzJackRabbit.Entities;
using Kriptok.Views.Gdip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Core.Wapons
{
    class Blaster : WeaponBase
    {
        public Blaster(Jazz jazz) : base(jazz)
        {
        }

        protected override void Shoot(Jazz jazz, Vector3F location, FlipEnum flip)
        {
            jazz.Blaster(location, flip);
        }
    }
}
