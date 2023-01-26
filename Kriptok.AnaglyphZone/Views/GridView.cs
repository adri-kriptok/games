using Kriptok.Drawing;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Views
{
    internal class GridView : GdipShapeView
    {
        public GridView() : base(Array.Empty<VertexBase>(), shapes =>
        {
            var length = AZConsts.MaxGridZ - AZConsts.MinGridZ + 1;

            var length2 = length * 2 + 3;

            var x0 = -length2 / 2 + 1;
            var x1 = length2 / 2 + 1;

            var supPlane = AZConsts.CamHeight * 2;

            AddPlane(shapes, AZConsts.MinGridZ, AZConsts.MaxGridZ, x0, x1, 0f);
            AddPlane(shapes, AZConsts.MinGridZ, AZConsts.MaxGridZ, x0, x1, AZConsts.CamHeight * 2);


            for (int i = x0; i < x1; i++)
            {
                var x = i * AZConsts.GridSize;
                shapes.Add(Strokes.Fuchsia,
                    new Vertex3(AZConsts.MinGridZ * AZConsts.GridSize, supPlane, x),
                    new Vertex3(AZConsts.MinGridZ * AZConsts.GridSize, 0f,       x));
            }

            for (int i = (int)AZConsts.GridSize; i < AZConsts.CamHeight * 2; i+= (int)AZConsts.GridSize)
            {
                var y = i;
                shapes.Add(Strokes.Fuchsia,
                    new Vertex3(AZConsts.MinGridZ * AZConsts.GridSize, y, x0 * AZConsts.GridSize),
                    new Vertex3(AZConsts.MinGridZ * AZConsts.GridSize, y, x1 * AZConsts.GridSize));
            }
        })
        {
        }

        private static void AddPlane(GdipShapeCollectionInitializer shapes, int minZ, int maxZ, int x0, float x1, float height)
        {
            for (int i = minZ; i < maxZ; i++)
            {
                var y = i * AZConsts.GridSize;
                shapes.Add(Strokes.Fuchsia,
                    new Vertex3(y , height, x0 * AZConsts.GridSize),
                    new Vertex3(y , height, x1 * AZConsts.GridSize));
            }

            for (int i = x0; i < x1; i++)
            {
                var x = i * AZConsts.GridSize;
                shapes.Add(Strokes.Fuchsia,
                    new Vertex3(minZ * AZConsts.GridSize, height, x),
                    new Vertex3(maxZ * AZConsts.GridSize, height, x));
            }
        }

        public override float GetPriority() => float.MinValue;
    }
}
