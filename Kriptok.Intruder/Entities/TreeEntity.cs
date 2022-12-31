using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Entities.Terrain;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Partitioned.Wld;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities
{
    internal class TreeEntity : EntityBase<TreeView>
    {
        private readonly ITerrainRegion map;

        public TreeEntity(ITerrainRegion map, Vector3F location)
            : base(new TreeView(map.GetFloorNormal(location.XY())))
        {
            this.map = map;

            Location = location;
            Radius = 60;

            // View.ScaleX = 3f + Rand.NextF(-0.25f, 0.25f);
            // View.ScaleY = 3f + Rand.NextF(-0.25f, 0.25f);
        }

        protected override void OnFrame()
        {
        }
    }

    internal class TreeView : FlatCircleView
    {
        public TreeView() : this(Vector3F.U001)
        {
        }

        public TreeView(Vector3F groundNormal)
            : base(8, Material.Get(TerrainShadow.ShadowColor, 1f, 0f, 0))
        {
            ScaleTransform(300f);

            var arcCosX = (float)Math.Acos(groundNormal.X) - MathHelper.HalfPIF;
            var arcCosY = (float)Math.Acos(groundNormal.Y) - MathHelper.HalfPIF;

            RotateTransform(0f, -MathHelper.HalfPIF, 0f);
            RotateTransform(-MathHelper.HalfPIF, 0f, 0f);

            RotateTransform(-arcCosY, 0f, 0f);
            RotateTransform(0f, arcCosX, 0f);

            RotateTransform(0f, MathHelper.HalfPIF, 0f);
            RotateTransform(0f, 0f, MathHelper.HalfPIF);

            SwapAllFaces();

            Add(new TreeShape(GetVertices().First()));
        }

        private class TreeShape : Particle<GdipSprite>
        {
            public TreeShape(VertexBase vertex) : base(new GdipSprite(typeof(TreeView).Assembly, "Assets.Images.Trees.Tree00.png")
            {
                Center = new PointF(0.5f, 0.99f),
                ScaleX = 3f + (float)Math.Cos(vertex.GetLocation().X) * 0.25f,
                ScaleY = 3f + (float)Math.Sin(vertex.GetLocation().Y) * 0.25f
            }, vertex)
            {
            }

            public override float GetPriority(IProjector projector) => int.MaxValue;            
        }
    }
}
