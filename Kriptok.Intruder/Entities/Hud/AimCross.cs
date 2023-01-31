using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Gdip.Base;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Intruder.Entities.Hud.AimCross;

namespace Kriptok.Intruder.Entities.Hud
{
    internal class AimCross : EntityBase<AimCrossView>
    {
        public AimCross() : base(new AimCrossView())
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Location.X = h.RegionSize.Width / 2;
            Location.Y = (h.RegionSize.Height - IntruderConsts.HudHeight) / 2;
        }

        protected override void OnFrame()
        {
        }

        internal class AimCrossView : CustomGdipViewBase
        {
            private Pen pen;

            public AimCrossView() : base(new SizeF(4f, 4f))
            {
                this.pen = new Pen(Color.White.SetAlpha(128), 1f);
            }

            protected override void Render(Graphics g)
            {
                g.DrawLine(pen, 2f, 0f, 2f, 4f);
                g.DrawLine(pen, 0f, 2f, 1f, 2f);
                g.DrawLine(pen, 3f, 2f, 4f, 2f);
            }

            public override void Dispose()
            {
                base.Dispose();

                if (pen != null)
                {
                    pen.Dispose();
                    pen = null;
                }
            }
        }
    }
}
