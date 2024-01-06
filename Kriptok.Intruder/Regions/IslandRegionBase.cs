using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Intruder.Maps;
using Kriptok.Mapping.Partitioned.Terrain;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Backgrounds;
using Kriptok.Regions.Pseudo3D.Partitioned.Base;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using Kriptok.Sdk.RM.VX;
using Kriptok.Sdk.RM.VX.Ace.Texsets;
using Kriptok.Views.Base;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Intruder.Regions
{
    /// <summary>
    /// Region para mapas de la isla.
    /// </summary>
    internal abstract class IslandRegionBase : Pseudo3DTerrainRegion
    {
        private readonly GdipHorizontalScrollingBackground<Pseudo3DTerrainRenderContext> skyBackground;

        public IslandRegionBase(Rectangle region, TerrainMapX map)
            : base(region, map, new ExteriorTexset())
        {
            // Pongo el cielo scrolleable.
            using (var back = FastBitmap.Load(RmVxResources.ParallaxAce("Ocean2.png")))
            {
                using (var halfBack = FastBitmap.CreateBySize(back.Size.Width, back.Size.Height / 2, null))
                {
                    halfBack.BlitImage(back, 0, 0);
                    this.skyBackground = new GdipHorizontalScrollingBackground<Pseudo3DTerrainRenderContext>(this, new HorizontalScrollArgs()
                    {
                        Repetitions = 1,
                        FovScale = 0.5f
                    }, halfBack);
                }
            }

            // Velocidad por default para las texturas animadas.
            TileAnimatedSpeed = 0.06f;        
        }

        protected override void Render(Pseudo3DTerrainRenderContext context, RenderizableHandler[] renderizables)
        {            
#if !DEBUG
            skyBackground.Render(context, (context.YShearing - 84f).Round());
#endif

            base.Render(context, renderizables.Take(50).ToArray());
        }

        protected override M9PartitionSideBase CreateSide(M9PartitionBase owner, 
            TerrainPartitionSideX sideX, M9Builder builder, M9Vertex v0, M9Vertex v1, int index)
        {
            if (sideX.Wall == null && sideX.PartitionToId == WldRegion.NoPartitionId)
            {
                sideX.Wall = new TerrainPartitionSideWallX()
                {
                    TextureId = ExteriorTexset.OceanTexture
                };

                return new M9PartitionSideToHorizon(owner, sideX, builder, v0, v1, index)
                {
                    TextureScale = ExteriorTexset.OceanTextureScale 
                };
            }
            return base.CreateSide(owner, sideX, builder, v0, v1, index);
        }
    }
}
