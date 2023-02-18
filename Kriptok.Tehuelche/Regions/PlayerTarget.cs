using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Tehuelche.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Regions
{
    internal class PlayerTarget : ScrollTarget
    {
        private readonly PlayerHelicopterAxonometric target;

        public PlayerTarget(PlayerHelicopterAxonometric target) : base(target)
        {
            this.target = target;
        }

        public override Vector2F GetLocation2D()
        {
            return base.GetLocation2D().Plus(target.GetRandomShake());
        }
    }
}
