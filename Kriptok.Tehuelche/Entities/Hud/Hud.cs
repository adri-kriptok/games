using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Entities.Hud
{
    internal class Hud : EntityBase<SpriteView>
    {
        public Hud() : base(new SpriteView(typeof(Hud).Assembly, "Assets.Images.Hud.Hud.png"))
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Location.X = h.RegionSize.Width / 2;
            Location.Y = h.RegionSize.Height - View.GetRectangle().Height / 2;
        }

        protected override void OnFrame()
        {
        }
    }
}
