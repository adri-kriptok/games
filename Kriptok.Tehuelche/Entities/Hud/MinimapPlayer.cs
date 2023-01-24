using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Entities.Hud
{
    internal class MinimapPlayer : EntityBase<PolygonView>
    {
        private readonly PlayerHelicopter player;
        private readonly float scale;

        public MinimapPlayer(PlayerHelicopter player, float scale) : base(new ArrowView())
        {
            this.player = player;
            this.scale = 1f / scale;
        }

        protected override void OnFrame()
        {
            Angle.Z = player.Angle.Z;
        }

        public override Vector3F GetRenderLocation()
        {
            var loc = player.GetRenderLocation();
            return new Vector3F(loc.X * scale, loc.Y * scale, 0f);
        }

        private class ArrowView : PolygonView
        {
            public ArrowView() : base(new PointF[4]
            {
                new PointF(0f, 0f),
                new PointF(8f, 4f),
                new PointF(0f, 8f),
                new PointF(2f, 4f)
            }, new FillConfig(Color.Green), Strokes.Green)
            {
            }
        }
    }
}
