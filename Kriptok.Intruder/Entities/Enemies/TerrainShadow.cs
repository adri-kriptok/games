using Kriptok.Extensions;
using Kriptok.Entities.Base;
using Kriptok.Views.Gdip.Base;
using Kriptok.Views.Shapes;
using System.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using System;
using Kriptok.Helpers;
using static Kriptok.Intruder.Entities.TerrainShadow;
using Kriptok.Regions.Context.Base;
using System.Runtime.Remoting.Contexts;

namespace Kriptok.Intruder.Entities
{
    internal class TerrainShadow : EntityBase<ShadowView>
    {
        internal static readonly Color ShadowColor = Color.Black.SetAlpha(64);
        private readonly IPseudo3DTerrainRegion map;
        private readonly EntityBase owner;
        private Vector3F normal;

        public TerrainShadow(EntityBase owner, IPseudo3DTerrainRegion map, float radius) : base(new ShadowView())
        {
            this.map = map;
            this.owner = owner;

            View.ScaleTransform(radius);
        }

        protected override void OnFrame()
        {            
            if (!owner.IsAlive())
            {
                Die();
            }

            this.normal = map.GetFloorNormal(owner.GetLocation2D());

#if DEBUG            
            Angle.Y = 0f;
            Angle.Z = 0f;
#endif

            var arcCosX = (float)Math.Acos(normal.X) - MathHelper.HalfPIF;
            var arcCosY = (float)Math.Acos(normal.Y) - MathHelper.HalfPIF;

            Angle.X = -arcCosY;
            Angle.Y = -arcCosX;
        }

        public override Vector3F GetRenderLocation() => Location;

        internal class ShadowView : FlatCircleView
        {
            public ShadowView() : base(8, Material.Get(ShadowColor, 1f, 0f, 0))
            {
            }

            public override float GetPriority(IProjector context)
            {
                // Para que aparezca siempre detrás del dinosaurio.
                return base.GetPriority(context) + 1f;
            }
        }
    }
}

