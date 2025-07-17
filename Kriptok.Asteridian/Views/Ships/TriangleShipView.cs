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
            new PointF(  4f, 12f),
            new PointF(  0f, 18f),
            new PointF( 18f, 9f),
            new PointF(  0f, 0f),
            new PointF(  4f, 6f),
        };

        public TriangleShipView(Color color) : base(vertices, new FillConfig(Color.Black), Strokes.Get(color, 1f))
        {            
            //this.Rounded = true;
        }
    }
}
