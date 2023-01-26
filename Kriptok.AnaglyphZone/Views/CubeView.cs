using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Views
{
    internal class CubeView : GdipShapeView
    {
        public CubeView() : base(TexturedCubeView.GetPointsForCube(), shapes =>
        {            
            shapes.Add(Material.Black, Strokes.Fuchsia, 0, 1, 2, 3);
            shapes.Add(Material.Black, Strokes.Fuchsia, 1, 4, 3, 7);
            shapes.Add(Material.Black, Strokes.Fuchsia, 4, 5, 7, 6);
            shapes.Add(Material.Black, Strokes.Fuchsia, 5, 0, 6, 2);
            shapes.Add(Material.Black, Strokes.Fuchsia, 5, 4, 0, 1);
            shapes.Add(Material.Black, Strokes.Fuchsia, 2, 3, 6, 7);
        })
        {
        }
    }
}
