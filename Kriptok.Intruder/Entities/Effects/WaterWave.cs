using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities.Effects
{
    /// <summary>
    /// Ondas que se dibujan cuando el persona está en el agua.
    /// </summary>
    internal class WaterWave : EntityBase<WaterWaveView>
    {
        private const float maxRadius = 75f;
        internal const byte InitialAlpha = 128;
        private const byte maxAlpha = byte.MaxValue - InitialAlpha;
        private readonly float incSize;
        private readonly byte decAlpha;

        public WaterWave(EntityBase owner, Vector3F location) : base(new WaterWaveView())
        {
            View.ScaleX = owner.Radius * 1.5f;
            View.ScaleY = owner.Radius * 1.5f;            
            this.Location = location;

            var diffRadius = (maxRadius - owner.Radius);
            this.incSize = diffRadius * 0.0005f;
            this.decAlpha = (byte)(Math.Max(1, maxAlpha / diffRadius));
        }

        protected override void OnFrame()
        {
            // View.Scale.X *= (1.2f + Rand.NextF(-0.05f, 0.05f));/// incSize * Sys.TimeDelta;
            View.Scale.X += (incSize + Rand.NextF(-0.05f, 0.05f)) * Sys.TimeDelta;

            if (View.Scale.X > maxRadius)
            {
                Die();
            }

            View.Scale.Y = View.Scale.X;
            
            var diff = View.Alpha - decAlpha;
            if (diff < 0)
            {
                Die();
            }
            View.Alpha = (byte)diff;
        }
    }

    internal class WaterWaveView : GdipShapeView
    {
        private const int vertexCount = 16;
        private static readonly Color color = Color.CornflowerBlue.SetAlpha((byte)(WaterWave.InitialAlpha + Rand.Next(-10, 10)));
        private readonly ModifiableStroke stroke = new ModifiableStroke(color, 2f);

        public WaterWaveView() : base(CreateVertices())
        {          
        }

        protected override void Build(GdipShapeCollectionInitializer shapes)
        {
            base.Build(shapes);

            var verts = base.GetVertices().ToArray();

            for (int i = 1; i < vertexCount; i++)
            {
               shapes.Add(stroke, verts[i], verts[i - 1]);
            }

            shapes.Add(stroke, verts[0], verts[vertexCount - 1]);
        }

        /// <summary>
        /// Establece el valor de la transparencia del trazo.
        /// </summary>        
        public byte Alpha { get => stroke.Alpha; set => stroke.Alpha = value; }

        private static VertexBase[] CreateVertices() => vectors.Select(p => new Vertex3(p)).ToArray();

        private static readonly Vector3F[] vectors = CreateVectors();

        private static Vector3F[] CreateVectors()
        {      
            var list = new List<Vector3F>();

            var inc = (float)(Math.PI * 2) / vertexCount;
            var ang = 0f;

            for (int i = 0; i < vertexCount; i++, ang += inc)
            {
                list.Add(new Vector3F((float)Math.Cos(ang), 0f, (float)Math.Sin(ang)));
            }

            return list.ToArray();
        }

    }
}
