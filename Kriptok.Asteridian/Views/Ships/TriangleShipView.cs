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
            new PointF( 00f, 00f),
                new PointF( 18f, 08f),
                new PointF( 18f, 10f),
                //new PointF( 18f, 11f),
            new PointF( 00f, 18f),
            new PointF( 04f, 12f),
            new PointF( 04f, 06f),
        };

        public TriangleShipView(Color line, Color fill) : base(vertices, new FillConfig(fill), Strokes.Get(line, 1f))
        {            
            //this.Rounded = true;
        }
    }
}
