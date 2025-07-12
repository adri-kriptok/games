using Kriptok.Drawing.Algebra;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Entities.Weapons
{
    internal class BlasterShot : ShotBase
    {
        public BlasterShot(Vector3F location, FlipEnum flip) : base(location, GetView(), flip)
        {
        }

        private static SpriteView GetView()
        {
            return new SpriteView(typeof(BlasterShot).Assembly, "Assets.Entities.Weapons.Blaster.png");
        }
    }
}
