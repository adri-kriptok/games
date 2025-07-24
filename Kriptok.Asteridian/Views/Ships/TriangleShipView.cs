using Kriptok.Asteridian.Helpers;
using Kriptok.Drawing.Algebra;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Views.Ships
{
    class TriangleShipView : PolygonView
    {
        private static readonly PointF[] vertices = new PointF[]
        {
            new PointF( 00f, -9f),
            new PointF( 18f, -1f),
            new PointF( 18f, +1f),            
            new PointF( 00f, +9f),
            new PointF( 04f, +3f),
            new PointF( 04f, -3f),
        };

        public TriangleShipView(Color line, Color fill) : base(ViewHelper.Translate(vertices), new FillConfig(fill), Strokes.Get(line, 1f))
        {            
            //this.Rounded = true;
        }
    }
}
