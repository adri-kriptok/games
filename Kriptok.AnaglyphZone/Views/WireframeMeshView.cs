using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Views
{
    internal class WireframeMeshView : GdipShapeView
    {
        public static readonly IMaterial BlackMaterial = Material.Get(Color.Black);

        public WireframeMeshView(Assembly assembly, string resourceName) : this(Meshes.Build(assembly, resourceName))
        {            
        }

        private WireframeMeshView(MeshBuilderBase builder) : this(builder, builder.GetVertices())
        {            
        }

        private WireframeMeshView(MeshBuilderBase builder, IDictionary<MeshBuilderBase.MeshBuilderVertex, VertexBase> vertices)
            :base(vertices.Values.ToArray(), shapes =>
            {
                foreach (var item in builder.Faces)
                {
                    if (item.Vertices.Count() == 3)
                    {
                        shapes.Add(BlackMaterial, Strokes.Fuchsia,
                            vertices[item.Vertices[0]],
                            vertices[item.Vertices[1]],
                            vertices[item.Vertices[2]]);
                    }
                    else
                    {
                        Debugger.Break();
                    }
                }
            })
        {            
        }
    }
}
