using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Views.Gdip;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities.Hud
{
    internal class AimCross : EntityBase<TextView>
    {
        public AimCross() : base(new TextView(new SuperFont(new Font("Arial", 9), Color.White.SetAlpha(128)), "+"))
        {            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Location.X = h.RegionSize.Width / 2;
            Location.Y = (h.RegionSize.Height - IntruderConsts.HudHeight) / 2 - 1;
        }

        protected override void OnFrame()
        {            
        }
    }
}
