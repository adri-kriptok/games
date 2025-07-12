using Kriptok.Drawing.Algebra;
using Kriptok.JazzJackRabbit.Entities;
using Kriptok.Views.Gdip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Core
{
    class Toaster : WeaponBase
    {
        public Toaster(Jazz jazz) : base(jazz)
        {
        }

        protected override void Shoot(Jazz jazz, Vector3F location, FlipEnum flip)
        {
            jazz.Toaster(location, flip);
        }
    }
}
