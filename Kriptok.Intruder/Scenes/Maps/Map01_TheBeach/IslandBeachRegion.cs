using Kriptok.Common.Base;
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
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain.Views;
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
    internal class IslandBeachRegion : IslandRegionBase
    {
        private readonly M9Vertex shoreVertex0;
        private readonly M9Vertex shoreVertex1;
        private readonly M9Vertex shoreVertex2;
        private readonly float shoreVertex0X;
        private readonly float shoreVertex1X;
        private readonly float shoreVertex2X;
        private float shoreEffect = 0f;

        private readonly ShoreEffect shoreEffectLocation = new ShoreEffect();

        public IslandBeachRegion(Rectangle region, TerrainMapX map) : base(region, map)
        {
            // Busco los dos pivotes de la playa para generar el efecto de las olas.
            var shoresVertices = base.FindVertices(p =>
            {
                return p.GetMetadata(IntruderConsts.VertexDataName) == IntruderConsts.VertexDataValueShore;
            }).ToArray();

            this.shoreVertex0 = shoresVertices[0];
            this.shoreVertex1 = shoresVertices[1];
            this.shoreVertex2 = shoresVertices[2];
            this.shoreVertex0X = shoreVertex0.X;
            this.shoreVertex1X = shoreVertex1.X;
            this.shoreVertex2X = shoreVertex2.X;

            foreach (var parti in Partitions
                .Where(p => p.TextureId == ExteriorTexset.OceanTexture)
                .OfType<IM9HorizontalTextureComponent>())
            {
                parti.TextureLocation = shoreEffectLocation;
            }
        }

        protected override void Render(Pseudo3DTerrainRenderContext context, RenderizableHandler[] renderizables)
        {
            shoreEffect += 0.0025f + Rand.NextF() * 0.0025f;

            var cosShoreEffect = (float)Math.Cos(shoreEffect);
            var sinShoreEffect = (float)Math.Sin(shoreEffect);
            shoreVertex0.X = shoreVertex0X + 300f * cosShoreEffect;
            shoreVertex1.X = shoreVertex1X + 300f * cosShoreEffect;
            shoreVertex2.X = shoreVertex2X + 300f * cosShoreEffect;

            shoreEffectLocation.X = -100f * cosShoreEffect;
            shoreEffectLocation.Y = sinShoreEffect * 10f;

            base.Render(context, renderizables.Take(50).ToArray());
        }

        /// <summary>
        /// Busco las particiones "más horizontales" y más grandes para ver cuáles pueden albergar árboles.
        /// </summary>        
        public M9PartitionBase[] GetTreePartitions()
        {
            return Partitions
                .Where(p => p.TextureId.In(ExteriorTexset.GrassTexture, ExteriorTexset.Grass2Texture))
                .Where(p => 1f - p.Normal.Z < IntruderConsts.MaxSlopePlayerAbleToClimb)
                .Select(p => new { partition = p, area = p.GetArea2D() })
                .OrderByDescending(p => p.area)
                .Take(100)
                .Select(p => p.partition)
                .ToArray();
        }

        /// <summary>
        /// Busco las particiones "más horizontales" y más grandes para ver cuáles pueden albergar árboles.
        /// </summary>        
        public M9PartitionBase[] GetRaptorPartitions()
        {
            return Partitions
                .Where(p => p.TextureId.In(ExteriorTexset.GrassTexture))
                //.Where(p => 1f - p.Normal.Z < IntruderConsts.MaxSlopePlayerAbleToClimb)
                .Select(p => new { partition = p, area = p.GetArea2D() })
                .OrderByDescending(p => p.area)
                .Take(4)
                .Select(p => p.partition)
                .ToArray();
        }

        /// <summary>
        /// Busco las particiones "más horizontales" y más grandes para ver cuáles pueden albergar árboles.
        /// </summary>        
        public M9PartitionBase[] GetLakePartitions()
        {
            return Partitions
                .Where(p => p.TextureId.In(ExteriorTexset.LakeTexture))
                .Select(p => new { partition = p, area = p.GetArea2D() })
                .OrderByDescending(p => p.area)
                .Take(8)
                .Select(p => p.partition)
                .ToArray();
        }

        internal M9PartitionBase[] GetSmallerPartitions()
        {
            var filtered = Partitions
                .Where(p => p.TextureId.In(ExteriorTexset.GrassTexture, ExteriorTexset.Grass2Texture))
                .Where(p => 1f - p.Normal.Z < IntruderConsts.MaxSlopePlayerAbleToClimb)
                .Select(p => new { partition = p, area = p.GetArea2D() })
                .OrderByDescending(p => p.area)
                .OrderBy(p => p.area)
                .Take(300)
                .Select(p => p.partition);
            return filtered.ToArray();
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
                    TextureScale = ExteriorTexset.OceanTextureScale,
                    TextureLocation = shoreEffectLocation
                };
            }
            return base.CreateSide(owner, sideX, builder, v0, v1, index);
        }

        internal static TerrainMapX GetMap(Assembly assembly, string mapPath)
        {
            if (Path.GetExtension(mapPath).Equals(".terrx"))
            {
                return TerrainMapX.Load(assembly, mapPath);
            }
            else
            {
                var mesh = Meshes.Build(assembly, mapPath)
                    .RotateTransform(-(float)(Math.PI / 2), 0f, 0f)
                    .ScaleTransform(32, 32, -32)
                    .ScaleTransform(10, 10, 5);
                mesh.SwapAllFaces();

                // Traslado el mapa hacia arriba para que quede el agua a nivel Z = 0;
                mesh.TranslateZ(-mesh.Vertices.Min(p => p.GetLocationZ()));

                // Traslado todos los vértices para que queden en positivos.
                var minX = mesh.Vertices.Min(p => p.Location.X);
                var minY = mesh.Vertices.Min(p => p.Location.Y);

                mesh.TranslateXY(-minX, -minY);

                foreach (var face in mesh.Faces)
                {
                    var scale = ExteriorTexset.GetMaterialScale(face.MaterialId);
                    face.UV0 = new TextureMappingPoint(face.Vertices[0].GetLocation2D().Scale(scale));
                    face.UV1 = new TextureMappingPoint(face.Vertices[1].GetLocation2D().Scale(scale));
                    face.UV2 = new TextureMappingPoint(face.Vertices[2].GetLocation2D().Scale(scale));
                }

                var textures = ExteriorTexset.GetTextureMapping();

                var map = new TerrainMapX(mesh, textures.Select(p => p.Id).ToArray());

                // Establezco la ubicación de "preview", ya que lo voy a usar
                // para inicializar el personaje.
                map.Preview.X = -minX.Round();
                map.Preview.Y = -minY.Round();

                foreach (var item in map.Partitions)
                {
                    item.Shading = textures.Single(p => p.Id == item.TextureId).Shading;

                    var u0 = item.Side0.U - item.Side0.U % 1f;
                    var v0 = item.Side0.V - item.Side0.V % 1f;

                    item.Side0.U -= u0;
                    item.Side0.V -= v0;

                    item.Side1.U -= u0;
                    item.Side1.V -= v0;

                    item.Side2.U -= u0;
                    item.Side2.V -= v0;
                }

                foreach (var item in map.GetAllWalls())
                {
                    item.Shading = ShadingAlgorithmEnum.Garoud;

                    var scale = ExteriorTexset.GetTextureScaleV(item.TextureId);

                    var v0 = map.GetVertex(item.V0);
                    var v1 = map.GetVertex(item.V1);
                    var v2 = map.GetVertex(item.V2);
                    var v3 = map.GetVertex(item.V3);

                    var v2D0 = v0.GetLocation2D().Scale(scale);
                    var v2D1 = v1.GetLocation2D().Scale(scale);

                    var z0 = Math.Max(v0.Z, v1.Z);
                    var z1 = Math.Min(v2.Z, v3.Z);

                    var height = (z0 - z1) * scale;
                    var distance = Vector2F.GetDistance(v2D0, v2D1);

                    item.TextureU0 = v2D0.X;
                    item.TextureV0 = 0f;

                    item.TextureU1 = v2D0.X + distance;
                    item.TextureV1 = height;

                    //if (item.TextureId == 2101196 /* Wood-Fence */)
                    //{
                    //    item.Shading = ShadingAlgorithmEnum.Flat;
                    //}
                    //else
                    //{
                    //    item.Shading = ShadingAlgorithmEnum.Garoud;
                    //}
                }


                // Seteo la escala de las particiones con textura de océano.
                foreach (var beachPartition in map.Partitions
                    .Where(p => p.TextureId == ExteriorTexset.SandTexture))
                {
                    // Si la normal de la playa es completamente vertical.
                    if (beachPartition.GetNormal().Equals(Vector3F.U001))
                    {
                        // Le indico que sombree plano para que se pueda
                        // hacer el efecto de la costa.
                        beachPartition.Shading = ShadingAlgorithmEnum.Flat;
                        beachPartition.HorizontalTextureMappingScale = ExteriorTexset.OceanTextureScale;
                    }
                    else
                    {
                    }
                }

                // Seteo la escala de las particiones con textura de océano.
                foreach (var oceanPartition in map.Partitions
                    .Where(p => p.TextureId == ExteriorTexset.OceanTexture))
                {
                    oceanPartition.HorizontalTextureMappingScale = ExteriorTexset.OceanTextureScale;
                }

                // Busco los dos pivotes de la playa para generar el efecto de las olas.
                var shoresVertices = map.Partitions.Where(p => p.TextureId == ExteriorTexset.SandTexture)
                    .SelectMany(p => p.GetVertices())
                    .Distinct()
                    .OrderBy(p => p.X)
                    .Take(3)
                    .ToArray();

                foreach (var vert in shoresVertices)
                {
                    vert[IntruderConsts.VertexDataName] = IntruderConsts.VertexDataValueShore;
                }

                // Busco los tres pivotes de donde termina la cascada.
                var waterfallVertices = map.Partitions.Where(p => p.TextureId == ExteriorTexset.RiverTexture)
                    .SelectMany(p => p.GetWalls().Where(w => w.TextureId.In(ExteriorTexset.WaterFallTexture, ExteriorTexset.WaterFall2Texture)))
                    .SelectMany(p => p.GetVertices())
                    .Distinct()
                    .OrderBy(p => p.Z)
                    .Take(3)
                    .ToArray();

                foreach (var vert in waterfallVertices)
                {
                    vert[IntruderConsts.VertexDataName] = IntruderConsts.VertexDataValueWaterfall;
                }


#if DEBUG
                map.Save(Path.ChangeExtension(mapPath, ".terrx"));
#endif
                return map;
            }
        }

        /// <summary>
        /// Busco los dos pivotes de la cascada para agregar el efecto de la espuma
        /// </summary>        
        internal M9Vertex[] GetWaterfallVertices() => base.FindVertices(p =>
        {
            return p.GetMetadata(IntruderConsts.VertexDataName) == IntruderConsts.VertexDataValueWaterfall;
        }).ToArray();

        private class ShoreEffect : ILocalizable2D
        {
            public float X;
            public float Y;

            public Vector2F GetLocation2D() => new Vector2F(X, Y);
        }
    }
}
